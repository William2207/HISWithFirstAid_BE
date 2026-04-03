using FirstAidAPI.Repository;
using FirstAidAPI.DTO.Invoice;
using FirstAidAPI.Models;
using FirstAidAPI.Enums;

namespace FirstAidAPI.Service.Implement
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMedicalServiceRepository _medicalServiceRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository, IMedicalServiceRepository medicalServiceRepository, IAppointmentRepository appointmentRepository)
        {
            _invoiceRepository = invoiceRepository;
            _medicalServiceRepository = medicalServiceRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<CreateInvoiceResponseDto> CreateInvoiceAsync(CreateInvoiceDto createInvoiceDto)
        {
            // Validate appointment
            if (createInvoiceDto.AppointmentId == null)
                throw new ArgumentException("AppointmentId is required.");

            var appointment = await _appointmentRepository.GetByIdAsync(createInvoiceDto.AppointmentId.Value);
            if (appointment == null)
                throw new KeyNotFoundException("Appointment not found.");

            // Build InvoiceItems
            var items = new List<InvoiceItem>();

            // Thêm phí khám chuyên khoa từ Appointment
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
            foreach (var itemDto in createInvoiceDto.Items)
            {
                if (itemDto.MedicalServiceId == null) continue;

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

            //Tính tiền
            var subtotal = items.Sum(i => i.Amount);
            var total = subtotal - createInvoiceDto.Discount;

            // Tạo Invoice
            var invoice = new Invoice
            {
                AppointmentId = createInvoiceDto.AppointmentId,
                PatientId = appointment.PatientId,
                InvoiceNumber = GenerateInvoiceNumber(),
                Subtotal = subtotal,
                Discount = createInvoiceDto.Discount,
                Total = total,
                RemainingAmount = total,   // chưa thanh toán gì
                Status = OrderStatus.Pending,
                Items = items
            };

            await _invoiceRepository.AddAsync(invoice);

            return new CreateInvoiceResponseDto
            {
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                Subtotal = subtotal,
                Discount = createInvoiceDto.Discount,
                Total = total,
                Status = invoice.Status
            };
        }

        public string GenerateInvoiceNumber()
        {
            string date = DateTime.Now.ToString("yyyyMMdd");
            string uniquePart = Guid.NewGuid().ToString("N")[..8].ToUpper();
            return $"INV-{date}-{uniquePart}";
        }
    }
}
