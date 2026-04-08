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
        private readonly IMedicalServiceRepository _medicalServiceRepository;
        private readonly IMomoService _momoService;
        private readonly ILogger<InvoiceService> _logger;

        public InvoiceService(
            IInvoiceRepository invoiceRepository,
            IPaymentRepository paymentRepository,
            IMedicalServiceRepository medicalServiceRepository,
            IAppointmentRepository appointmentRepository,
            IMomoService momoService,
            ILogger<InvoiceService> logger)
        {
            _invoiceRepository = invoiceRepository;
            _paymentRepository = paymentRepository;
            _medicalServiceRepository = medicalServiceRepository;
            _appointmentRepository = appointmentRepository;
            _momoService = momoService;
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

            // Build InvoiceItems
            var items = await BuildInvoiceItemsAsync(appointment, createInvoiceDto.Items);

            // Tính tiền
            var (subtotal, total) = CalculateAmounts(items, createInvoiceDto.Discount);
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
                Status = OrderStatus.Pending,
                Items = items
            };

            await _invoiceRepository.AddAsync(invoice);

            // Gọi Momo để tạo payment URL
            var momoResponse = await _momoService.CreatePaymentAsync(new MomoCreatePaymentRequestDto
            {
                OrderNumber = invoiceNumber,
                Amount = (long)total,
                OrderDescription = $"Thanh toán hóa đơn khám bệnh {invoiceNumber}"
            });

            string? paymentUrl = null;
            if (momoResponse.ResultCode == 0)
            {
                paymentUrl = momoResponse.PayUrl;
            }

            _logger.LogInformation("Invoice created successfully. InvoiceNumber: {InvoiceNumber}, PatientId: {PatientId}, Total: {Total}",
                invoiceNumber, appointment.PatientId, total);

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

            // Kiểm tra payment đã tồn tại chưa
            var existingPayment = await _paymentRepository.GetByTransactionIdAsync(transactionId);
            if (existingPayment != null)
            {
                _logger.LogWarning("Payment already exists for transaction {TransactionId}", transactionId);
                throw new InvalidOperationException($"Payment already recorded for transaction {transactionId}");
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

        /// <summary>
        /// Tạo danh sách InvoiceItems từ appointment và medical services
        /// </summary>
        private async Task<List<InvoiceItem>> BuildInvoiceItemsAsync(
            Appointment appointment,
            List<CreateInvoiceItemDto> itemDtos)
        {
            var items = new List<InvoiceItem>();

            // Thêm phí khám chuyên khoa từ Appointment (luôn có)
            var speciality = appointment.Specialty;
            items.Add(new InvoiceItem
            {
                SpecilityId = speciality.Id,
                Description = speciality.Name,
                Quantity = 1,
                UnitPrice = speciality.Price,
                Amount = speciality.Price
            });

            // Thêm các dịch vụ y tế nếu có (xét nghiệm, chẩn đoán...)
            foreach (var itemDto in itemDtos)
            {
                if (itemDto.MedicalServiceId == null)
                    continue;

                var service = await _medicalServiceRepository.GetByIdAsync(itemDto.MedicalServiceId.Value);
                if (service == null)
                    throw new KeyNotFoundException($"MedicalService {itemDto.MedicalServiceId} not found.");

                items.Add(new InvoiceItem
                {
                    MedicalServiceId = service.Id,
                    Description = service.Name,      // snapshot tên lúc tạo
                    Quantity = itemDto.Quantity,
                    UnitPrice = service.Price,        // snapshot giá lúc tạo
                    Amount = service.Price * itemDto.Quantity
                });
            }

            return items;
        }

        /// <summary>
        /// Tính toán tổng tiền và tổng cộng từ danh sách items
        /// </summary>
        private (decimal subtotal, decimal total) CalculateAmounts(
            List<InvoiceItem> items,
            decimal discount)
        {
            decimal subtotal = items.Sum(i => i.Amount);
            decimal total = subtotal - discount;

            if (total < 0)
            {
                _logger.LogWarning("Discount exceeds subtotal. Subtotal: {Subtotal}, Discount: {Discount}",
                    subtotal, discount);
                throw new ArgumentException("Discount cannot exceed subtotal.");
            }

            return (subtotal, total);
        }

        public string GenerateInvoiceNumber()
        {
            string date = DateTime.Now.ToString("yyyyMMdd");
            string uniquePart = Guid.NewGuid().ToString("N")[..8].ToUpper();
            return $"INV-{date}-{uniquePart}";
        }
    }
}
