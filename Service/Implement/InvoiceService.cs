using FirstAidAPI.Repository;
using FirstAidAPI.DTO.Invoice;
using FirstAidAPI.DTO.Payment;
using FirstAidAPI.Models;
using FirstAidAPI.Enums;
using FirstAidAPI.Service.Payment;
using Microsoft.Extensions.Logging;

namespace FirstAidAPI.Service.Implement
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ILabOrderRepository _labOrderRepository;
        private readonly IMomoService _momoService;
        private readonly IPatientRepository _patientRepository;
        private readonly ILogger<InvoiceService> _logger;

        public InvoiceService(
            IInvoiceRepository invoiceRepository,
            IPaymentRepository paymentRepository,
            IAppointmentRepository appointmentRepository,
            ILabOrderRepository labOrderRepository,
            IMomoService momoService,
            IPatientRepository patientRepository,
            ILogger<InvoiceService> logger)
        {
            _invoiceRepository = invoiceRepository;
            _paymentRepository = paymentRepository;
            _appointmentRepository = appointmentRepository;
            _labOrderRepository = labOrderRepository;
            _momoService = momoService;
            _patientRepository = patientRepository;
            _logger = logger;
        }

        public async Task<CreateInvoiceResponseDto> CreateInvoiceAsync(CreateInvoiceDto dto)
        {
            // Xác định loại invoice: Appointment hoặc LabOrder
            if (dto.LabOrderId.HasValue)
                return await CreateLabOrderInvoiceAsync(dto);

            return await CreateAppointmentInvoiceAsync(dto);
        }

        private async Task<CreateInvoiceResponseDto> CreateAppointmentInvoiceAsync(CreateInvoiceDto dto)
        {
            if (dto.AppointmentId == null)
                throw new ArgumentException("AppointmentId là bắt buộc.");

            var appointment = await _appointmentRepository.GetByIdAsync(dto.AppointmentId.Value)
                ?? throw new KeyNotFoundException("Không tìm thấy lịch hẹn.");

            var existingInvoice = await _invoiceRepository.GetByAppointmentIdAsync(dto.AppointmentId.Value);
            if (existingInvoice != null)
                throw new InvalidOperationException(
                    $"Hóa đơn đã tồn tại cho lịch hẹn này (Số: {existingInvoice.InvoiceNumber}).");

            decimal subtotal = appointment.Specialty?.Price ?? 0;
            decimal total = subtotal - dto.Discount;
            if (total < 0)
                throw new ArgumentException("Giảm giá không thể vượt quá tổng tiền.");

            var invoiceNumber = GenerateInvoiceNumber();
            var invoice = new Invoice
            {
                AppointmentId = dto.AppointmentId,
                PatientId = appointment.PatientId,
                InvoiceNumber = invoiceNumber,
                Subtotal = subtotal,
                Discount = dto.Discount,
                Total = total,
                RemainingAmount = total,
                Status = OrderStatus.Pending
            };

            await _invoiceRepository.AddAsync(invoice);

            return await ProcessPaymentAsync(invoice, dto.PaymentMethod,
                $"Thanh toán hóa đơn khám bệnh {invoiceNumber}",
                dto.Discount, subtotal);
        }

        private async Task<CreateInvoiceResponseDto> CreateLabOrderInvoiceAsync(CreateInvoiceDto dto)
        {
            var labOrder = await _labOrderRepository.GetByIdAsync(dto.LabOrderId!.Value)
                ?? throw new KeyNotFoundException($"Không tìm thấy chỉ định {dto.LabOrderId.Value}.");

            if (labOrder.Status != LabOrderStatus.Pending)
                throw new InvalidOperationException("Chỉ có thể tạo hóa đơn cho chỉ định đang ở trạng thái Chờ thanh toán.");

            var existingInvoice = await _invoiceRepository.GetByLabOrderIdAsync(dto.LabOrderId.Value);
            if (existingInvoice != null)
                throw new InvalidOperationException(
                    $"Hóa đơn đã tồn tại cho chỉ định này (Số: {existingInvoice.InvoiceNumber}).");

            decimal subtotal = labOrder.Items.Sum(i => i.Amount);
            decimal total = subtotal - dto.Discount;
            if (total < 0)
                throw new ArgumentException("Giảm giá không thể vượt quá tổng tiền.");

            var invoiceNumber = GenerateInvoiceNumber();
            var invoice = new Invoice
            {
                LabOrderId = dto.LabOrderId,
                PatientId = labOrder.PatientId,
                InvoiceNumber = invoiceNumber,
                Subtotal = subtotal,
                Discount = dto.Discount,
                Total = total,
                RemainingAmount = total,
                Status = OrderStatus.Pending
            };

            await _invoiceRepository.AddAsync(invoice);

            string? paymentUrl = null;

            if (dto.PaymentMethod == PaymentMethod.Cash)
            {
                invoice.Status = OrderStatus.Completed;
                invoice.PaidAmount = total;
                invoice.RemainingAmount = 0;
                invoice.PaidAt = DateTime.UtcNow;
                await _invoiceRepository.UpdateAsync(invoice);

                await CreatePaymentRecordAsync(invoice, PaymentMethod.Cash,
                    "CASH-" + Guid.NewGuid().ToString("N")[..8].ToUpper());

                // Cập nhật trạng thái LabOrder → Paid
                labOrder.Status = LabOrderStatus.Paid;
                await _labOrderRepository.UpdateAsync(labOrder);

                _logger.LogInformation(
                    "LabOrder Invoice (Cash) completed. InvoiceNumber: {InvoiceNumber}, LabOrderId: {LabOrderId}, Total: {Total}",
                    invoiceNumber, labOrder.Id, total);
            }
            else
            {
                var baseUrl = _momoService.GetBaseUrl();
                var momoResponse = await _momoService.CreatePaymentAsync(new MomoCreatePaymentRequestDto
                {
                    OrderNumber = invoiceNumber,
                    Amount = (long)total,
                    OrderDescription = $"Thanh toán chỉ định xét nghiệm {invoiceNumber}",
                    ReturnUrl = $"{baseUrl}/api/invoices/momo-callback"
                });

                if (momoResponse.ResultCode == 0)
                    paymentUrl = momoResponse.PayUrl;

                _logger.LogInformation(
                    "LabOrder Invoice (Momo) created. InvoiceNumber: {InvoiceNumber}, LabOrderId: {LabOrderId}, Total: {Total}",
                    invoiceNumber, labOrder.Id, total);
            }

            return new CreateInvoiceResponseDto
            {
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                Subtotal = subtotal,
                Discount = dto.Discount,
                Total = total,
                Status = invoice.Status,
                PaymentUrl = paymentUrl
            };
        }

        private async Task<CreateInvoiceResponseDto> ProcessPaymentAsync(
            Invoice invoice, PaymentMethod? paymentMethod,
            string momoDescription, decimal discount, decimal subtotal)
        {
            string? paymentUrl = null;

            if (paymentMethod == PaymentMethod.Cash)
            {
                invoice.Status = OrderStatus.Completed;
                invoice.PaidAmount = invoice.Total;
                invoice.RemainingAmount = 0;
                invoice.PaidAt = DateTime.UtcNow;
                await _invoiceRepository.UpdateAsync(invoice);

                await CreatePaymentRecordAsync(invoice, PaymentMethod.Cash,
                    "CASH-" + Guid.NewGuid().ToString("N")[..8].ToUpper());

                _logger.LogInformation(
                    "Cash Invoice completed. InvoiceNumber: {InvoiceNumber}, PatientId: {PatientId}, Total: {Total}",
                    invoice.InvoiceNumber, invoice.PatientId, invoice.Total);
            }
            else
            {
                var baseUrl = _momoService.GetBaseUrl();
                var momoResponse = await _momoService.CreatePaymentAsync(new MomoCreatePaymentRequestDto
                {
                    OrderNumber = invoice.InvoiceNumber,
                    Amount = (long)invoice.Total,
                    OrderDescription = momoDescription,
                    ReturnUrl = $"{baseUrl}/api/invoices/momo-callback"
                });

                if (momoResponse.ResultCode == 0)
                    paymentUrl = momoResponse.PayUrl;

                _logger.LogInformation(
                    "Momo Invoice created. InvoiceNumber: {InvoiceNumber}, PatientId: {PatientId}, Total: {Total}",
                    invoice.InvoiceNumber, invoice.PatientId, invoice.Total);
            }

            return new CreateInvoiceResponseDto
            {
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                Subtotal = subtotal,
                Discount = discount,
                Total = invoice.Total,
                Status = invoice.Status,
                PaymentUrl = paymentUrl
            };
        }

        private async Task CreatePaymentRecordAsync(Invoice invoice, PaymentMethod method, string transactionId)
        {
            var payment = new Models.Payment
            {
                InvoiceId = invoice.Id,
                PatientId = invoice.PatientId,
                Amount = invoice.Total,
                PaymentMethod = method,
                Status = PaymentStatus.Completed,
                TransactionId = transactionId,
                PaidAt = DateTime.UtcNow
            };
            await _paymentRepository.AddAsync(payment);
        }

        public async Task CompleteInvoiceAsync(int invoiceId, string transactionId)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(invoiceId)
                ?? throw new KeyNotFoundException($"Invoice {invoiceId} not found.");

            // Kiểm tra invoice đã hoàn tất chưa (Idempotency)
            if (invoice.Status == OrderStatus.Completed)
            {
                _logger.LogInformation("Invoice {InvoiceNumber} already completed.", invoice.InvoiceNumber);
                return;
            }

            // Cập nhật LabOrder.Status → Paid nếu invoice liên kết với LabOrder
            if (invoice.LabOrderId.HasValue)
            {
                var labOrder = await _labOrderRepository.GetByIdAsync(invoice.LabOrderId.Value);
                if (labOrder != null && labOrder.Status == LabOrderStatus.Pending)
                {
                    labOrder.Status = LabOrderStatus.Paid;
                    await _labOrderRepository.UpdateAsync(labOrder);
                }
            }

            // Kiểm tra payment đã tồn tại chưa
            var existingPayment = await _paymentRepository.GetByTransactionIdAsync(transactionId);
            if (existingPayment != null)
            {
                _logger.LogWarning("Payment already exists for transaction {TransactionId}. Marking invoice as completed if not already.", transactionId);
                
                // Đảm bảo sync trạng thái invoice nếu cần
                invoice.Status = OrderStatus.Completed;
                await _invoiceRepository.UpdateAsync(invoice);
                return;
            }

            // Tạo Payment record
            var payment = new Models.Payment
            {
                InvoiceId = invoice.Id,
                PatientId = invoice.PatientId,
                Amount = invoice.Total,
                PaymentMethod = PaymentMethod.Momo,
                Status = PaymentStatus.Completed,
                TransactionId = transactionId,
                PaidAt = DateTime.UtcNow
            };

            await _paymentRepository.AddAsync(payment);

            // Update Invoice status
            invoice.Status = OrderStatus.Completed;
            invoice.PaidAmount = invoice.Total;
            invoice.RemainingAmount = 0;
            invoice.PaidAt = DateTime.UtcNow;

            await _invoiceRepository.UpdateAsync(invoice);

            _logger.LogInformation("Invoice completed. InvoiceNumber: {InvoiceNumber}, TransactionId: {TransactionId}, Amount: {Amount}",
                invoice.InvoiceNumber, transactionId, invoice.Total);
        }

        public async Task FailInvoiceAsync(int invoiceId)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(invoiceId)
                ?? throw new KeyNotFoundException($"Invoice {invoiceId} not found.");

            if (invoice.Status != OrderStatus.Pending)
            {
                _logger.LogWarning("Cannot fail invoice {InvoiceNumber}. Current status: {Status}",
                    invoice.InvoiceNumber, invoice.Status);
                throw new InvalidOperationException($"Only pending invoices can be failed.");
            }

            invoice.Status = OrderStatus.Cancelled;
            await _invoiceRepository.UpdateAsync(invoice);

            // Hủy appointment khi payment fail
            if (invoice.AppointmentId.HasValue)
            {
                var appointment = await _appointmentRepository.GetByIdAsync(invoice.AppointmentId.Value);
                if (appointment != null)
                {
                    await _appointmentRepository.DeleteAsync(appointment);
                    _logger.LogInformation("Cancelled appointment {AppointmentId} due to failed invoice {InvoiceNumber}",
                        invoice.AppointmentId.Value, invoice.InvoiceNumber);
                }
            }

            _logger.LogInformation("Invoice failed. InvoiceNumber: {InvoiceNumber}", invoice.InvoiceNumber);
        }

        // Được gọi từ InvoicesController sau khi lookup bằng InvoiceNumber
        public async Task<CreateInvoiceResponseDto?> GetByInvoiceNumberAsync(string invoiceNumber)
        {
            var invoice = await _invoiceRepository.GetByInvoiceNumberAsync(invoiceNumber);
            if (invoice == null) return null;

            return new CreateInvoiceResponseDto
            {
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                Subtotal = invoice.Subtotal,
                Discount = invoice.Discount,
                Total = invoice.Total,
                Status = invoice.Status
            };
        }

        public async Task<IEnumerable<PatientInvoiceDto>> GetInvoicesByPatientAsync(int patientId)
        {
            var invoices = await _invoiceRepository.GetByPatientIdAsync(patientId);
            
            return invoices.Select(i => new PatientInvoiceDto
            {
                Id = i.Id,
                InvoiceNumber = i.InvoiceNumber,
                Description = i.Appointment?.Specialty?.Name ?? "Thanh toán dịch vụ y tế",
                Total = i.Total,
                Status = i.Status,
                CreatedAt = i.CreatedAt,
                PaidAt = i.PaidAt,
                PaymentMethod = i.Payments.OrderByDescending(p => p.PaidAt).FirstOrDefault()?.PaymentMethod.ToString() ?? "Chưa có",
            }).ToList();
        }

        public async Task<IEnumerable<PatientInvoiceDto>> GetInvoicesByUserIdAsync(int userId)
        {
            var patient = await _patientRepository.GetByUserIdAsync(userId);
            if (patient == null)
            {
                throw new KeyNotFoundException("Không tìm thấy hồ sơ bệnh nhân.");
            }

            return await GetInvoicesByPatientAsync(patient.Id);
        }

        public async Task ProcessMomoPaymentAsync(MomoCallbackDto callback)
        {
            var invoice = await _invoiceRepository.GetByInvoiceNumberAsync(callback.OrderId);
            if (invoice == null)
            {
                _logger.LogError("Invoice {InvoiceNumber} not found during Momo callback processing.", callback.OrderId);
                throw new KeyNotFoundException($"Invoice {callback.OrderId} not found.");
            }

            if (callback.ResultCode == 0)
            {
                // Thanh toán thành công -> Hoàn tất hóa đơn và tạo Payment record
                await CompleteInvoiceAsync(invoice.Id, callback.TransId.ToString());
            }
            else
            {
                // Thanh toán thất bại -> Hủy hóa đơn
                _logger.LogWarning("Payment failed for Invoice {InvoiceNumber}. ResultCode: {ResultCode}, Message: {Message}",
                    invoice.InvoiceNumber, callback.ResultCode, callback.Message);

                invoice.Status = OrderStatus.Cancelled;
                await _invoiceRepository.UpdateAsync(invoice);

                // Hủy appointment liên quan nếu có
                if (invoice.AppointmentId.HasValue)
                {
                    var appointment = await _appointmentRepository.GetByIdAsync(invoice.AppointmentId.Value);
                    if (appointment != null)
                    {
                        await _appointmentRepository.DeleteAsync(appointment);
                        _logger.LogInformation("Deleted appointment {AppointmentId} due to failed payment for invoice {InvoiceNumber}",
                            invoice.AppointmentId.Value, invoice.InvoiceNumber);
                    }
                }
            }
        }

        public string GenerateInvoiceNumber()
        {
            string date = DateTime.Now.ToString("yyyyMMdd");
            string uniquePart = Guid.NewGuid().ToString("N")[..8].ToUpper();
            return $"INV-{date}-{uniquePart}";
        }
    }
}
