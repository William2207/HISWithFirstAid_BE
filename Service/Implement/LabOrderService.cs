using FirstAidAPI.DTO.LabOrder;
using FirstAidAPI.DTO.MedicalService;
using FirstAidAPI.Enums;
using FirstAidAPI.Models;
using FirstAidAPI.Repository;

namespace FirstAidAPI.Service.Implement
{
    public class LabOrderService : ILabOrderService
    {
        private readonly ILabOrderRepository _labOrderRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMedicalServiceRepository _medicalServiceRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly ILogger<LabOrderService> _logger;

        public LabOrderService(
            ILabOrderRepository labOrderRepository,
            IAppointmentRepository appointmentRepository,
            IMedicalServiceRepository medicalServiceRepository,
            IDoctorRepository doctorRepository,
            IPatientRepository patientRepository,
            ILogger<LabOrderService> logger)
        {
            _labOrderRepository = labOrderRepository;
            _appointmentRepository = appointmentRepository;
            _medicalServiceRepository = medicalServiceRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _logger = logger;
        }

        public async Task<LabOrderResponseDto> CreateLabOrderAsync(CreateLabOrderDto dto, int doctorId)
        {
            if (dto.Items == null || dto.Items.Count == 0)
                throw new ArgumentException("Chỉ định phải có ít nhất một dịch vụ.");

            var appointment = await _appointmentRepository.GetByIdAsync(dto.AppointmentId)
                ?? throw new KeyNotFoundException($"Không tìm thấy lịch hẹn {dto.AppointmentId}.");

            if (appointment.Status != AppointmentStatus.In_Progress)
                throw new InvalidOperationException("Chỉ có thể tạo chỉ định khi lịch hẹn đang trong trạng thái Đang khám.");

            var doctor = await _doctorRepository.GetByUserIdAsync(doctorId)
                ?? throw new KeyNotFoundException("Không tìm thấy thông tin bác sĩ.");

            var labOrderItems = await BuildLabOrderItemsAsync(dto.Items);

            var labOrder = new LabOrder
            {
                AppointmentId = dto.AppointmentId,
                DoctorId = doctor.Id,
                PatientId = appointment.PatientId,
                Status = LabOrderStatus.Pending,
                Items = labOrderItems,
                CreatedAt = DateTime.UtcNow
            };

            await _labOrderRepository.AddAsync(labOrder);

            _logger.LogInformation(
                "LabOrder created. LabOrderId: {LabOrderId}, AppointmentId: {AppointmentId}, DoctorId: {DoctorId}, Items: {ItemCount}",
                labOrder.Id, dto.AppointmentId, doctor.Id, labOrder.Items.Count);

            return MapToResponseDto(labOrder);
        }

        public async Task<List<LabOrderResponseDto>> GetByAppointmentIdAsync(int appointmentId)
        {
            var labOrders = await _labOrderRepository.GetByAppointmentIdAsync(appointmentId);
            return labOrders.Select(MapToResponseDto).ToList();
        }

        public async Task<List<LabOrderResponseDto>> GetByPatientIdAsync(int patientId)
        {
            var labOrders = await _labOrderRepository.GetByPatientIdAsync(patientId);
            return labOrders.Select(MapToResponseDto).ToList();
        }

        public async Task<List<LabOrderResponseDto>> GetByUserIdAsync(int userId)
        {
            var patient = await _patientRepository.GetByUserIdAsync(userId);
            if (patient == null)
            {
                throw new KeyNotFoundException("Không tìm thấy hồ sơ bệnh nhân.");
            }
            var labOrders = await _labOrderRepository.GetByPatientIdAsync(patient.Id);
            return labOrders.Select(MapToResponseDto).ToList();
        }

        public async Task<List<LabOrderResponseDto>> GetPendingLabOrdersAsync()
        {
            var labOrders = await _labOrderRepository.GetPendingWithNoInvoiceAsync();
            return labOrders.Select(MapToResponseDto).ToList();
        }

        public async Task<List<MedicalServiceDto>> GetAllMedicalServicesAsync()
        {
            var services = await _medicalServiceRepository.GetAllActiveAsync();
            return services.Select(s => new MedicalServiceDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Price = s.Price
            }).ToList();
        }

        // ── Private helpers ────────────────────────────────────────────────

        private async Task<List<LabOrderItem>> BuildLabOrderItemsAsync(
            List<LabOrderItemInputDto> itemDtos)
        {
            var items = new List<LabOrderItem>();

            foreach (var itemDto in itemDtos)
            {
                var service = await _medicalServiceRepository.GetByIdAsync(itemDto.MedicalServiceId)
                    ?? throw new KeyNotFoundException(
                        $"Dịch vụ y tế {itemDto.MedicalServiceId} không tồn tại hoặc không hoạt động.");

                items.Add(new LabOrderItem
                {
                    MedicalServiceId = service.Id,
                    UnitPrice = service.Price,
                    Quantity = itemDto.Quantity,
                    Amount = service.Price * itemDto.Quantity,
                    Note = itemDto.Note
                });
            }

            return items;
        }

        private static LabOrderResponseDto MapToResponseDto(LabOrder labOrder)
        {
            var patientName = labOrder.Appointment?.Patient?.FullNameDisplay ?? "N/A";
            var doctorName = labOrder.Appointment?.Doctor?.User?.FullName ?? "N/A";
            var totalAmount = labOrder.Items.Sum(i => i.Amount);

            return new LabOrderResponseDto
            {
                Id = labOrder.Id,
                AppointmentId = labOrder.AppointmentId,
                PatientId = labOrder.PatientId,
                PatientName = patientName,
                DoctorName = doctorName,
                Status = labOrder.Status,
                StatusLabel = GetStatusLabel(labOrder.Status),
                TotalAmount = totalAmount,
                CreatedAt = labOrder.CreatedAt,
                Items = labOrder.Items.Select(li => new LabOrderItemResponseDto
                {
                    Id = li.Id,
                    MedicalServiceId = li.MedicalServiceId,
                    ServiceName = li.MedicalService?.Name ?? "N/A",
                    Quantity = li.Quantity,
                    UnitPrice = li.UnitPrice,
                    Amount = li.Amount,
                    Note = li.Note,
                    ResultImageUrl = li.ResultImageUrl,
                    ResultNote = li.ResultNote,
                    ResultData = li.ResultData
                }).ToList()
            };
        }

        private static string GetStatusLabel(LabOrderStatus status) => status switch
        {
            LabOrderStatus.Pending => "Chờ thanh toán",
            LabOrderStatus.Paid => "Đã thanh toán",
            LabOrderStatus.Completed => "Đã hoàn thành",
            LabOrderStatus.Cancelled => "Đã hủy",
            _ => "Không xác định"
        };
    }
}
