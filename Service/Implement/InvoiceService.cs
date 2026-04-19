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
        private readonly IMomoService _momoService;
        private readonly IPatientRepository _patientRepository;
        private readonly ILogger<InvoiceService> _logger;

        public InvoiceService(
            IInvoiceRepository invoiceRepository,
            IPaymentRepository paymentRepository,
            IAppointmentRepository appointmentRepository,
            IMomoService momoService,
            IPatientRepository patientRepository,
            ILogger<InvoiceService> logger)
        {
            _invoiceRepository = invoiceRepository;
            _paymentRepository = paymentRepository;
            _appointmentRepository = appointmentRepository;
            _momoService = momoService;
            _patientRepository = patientRepository;
            _logger = logger;
        }

        public async Task<CreateInvoiceResponseDto> CreateInvoiceAsync(CreateInvoiceDto createInvoiceDto)
        {
            // Validate appointment
            if (createInvoiceDto.AppointmentId == null)
                throw new ArgumentException("AppointmentId is required.");

            var appointment = await _appointmentRepository.GetByIdAsync(createInvoiceDto.AppointmentId.Value);
            if (appointment == null)
                throw new KeyNotFoundException("Appointment not found.");

            // Kiểm tra invoice đã tồn tại chưa (tránh tạo duplicate)
            var existingInvoice = await _invoiceRepository.GetByAppointmentIdAsync(createInvoiceDto.AppointmentId.Value);
            if (existingInvoice != null)
                throw new InvalidOperationException(
                    $"Invoice đã tồn tại cho appointment này (Invoice số: {existingInvoice.InvoiceNumber}).");

            // Tính tiền
            decimal subtotal = appointment.Specialty?.Price ?? 0;
            decimal total = subtotal - createInvoiceDto.Discount;

            if (total < 0)
            {
                _logger.LogWarning("Discount exceeds subtotal. Subtotal: {Subtotal}, Discount: {Discount}",
                    subtotal, createInvoiceDto.Discount);
                throw new ArgumentException("Discount cannot exceed subtotal.");
            }

            var invoiceNumber = GenerateInvoiceNumber();

            // Tạo Invoice (Pending - chờ thanh toán)
            var invoice = new Invoice
            {
                AppointmentId = createInvoiceDto.AppointmentId,
                PatientId = appointment.PatientId,
                InvoiceNumber = invoiceNumber,
                Subtotal = subtotal,
                Discount = createInvoiceDto.Discount,
                Total = total,
                RemainingAmount = total,
                Status = OrderStatus.Pending
            };

            await _invoiceRepository.AddAsync(invoice);

            string? paymentUrl = null;

            if (createInvoiceDto.PaymentMethod == PaymentMethod.Cash)
            {
                invoice.Status = OrderStatus.Completed;
                invoice.PaidAmount = total;
                invoice.RemainingAmount = 0;
                invoice.PaidAt = DateTime.UtcNow;

                await _invoiceRepository.UpdateAsync(invoice);

                var payment = new Models.Payment
                {
                    InvoiceId = invoice.Id,
                    PatientId = invoice.PatientId,
                    Amount = invoice.Total,
                    PaymentMethod = PaymentMethod.Cash,
                    Status = PaymentStatus.Completed,
                    TransactionId = "CASH-" + Guid.NewGuid().ToString("N")[..8].ToUpper(),
                    PaidAt = DateTime.UtcNow
                };
                await _paymentRepository.AddAsync(payment);
                
                _logger.LogInformation("Cash Invoice created & completed. InvoiceNumber: {InvoiceNumber}, PatientId: {PatientId}, Total: {Total}",
                    invoiceNumber, appointment.PatientId, total);
            }
            else
            {
                // Gọi Momo để tạo payment URL
                var baseUrl = _momoService.GetBaseUrl();
                var momoResponse = await _momoService.CreatePaymentAsync(new MomoCreatePaymentRequestDto
                {
                    OrderNumber = invoiceNumber,
                    Amount = (long)total,
                    OrderDescription = $"Thanh toán hóa đơn khám bệnh {invoiceNumber}",
                    ReturnUrl = $"{baseUrl}/api/invoices/momo-callback"
                });

                if (momoResponse.ResultCode == 0)
                {
                    paymentUrl = momoResponse.PayUrl;
                }

                _logger.LogInformation("Invoice created successfully. InvoiceNumber: {InvoiceNumber}, PatientId: {PatientId}, Total: {Total}",
                    invoiceNumber, appointment.PatientId, total);
            }

            return new CreateInvoiceResponseDto
            {
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                Subtotal = subtotal,
                Discount = createInvoiceDto.Discount,
                Total = total,
                Status = invoice.Status,
                PaymentUrl = paymentUrl
            };
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
