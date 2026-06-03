using FirstAidAPI.DTO.LabOrder;
using FirstAidAPI.DTO.MedicalService;

namespace FirstAidAPI.Service
{
    public interface ILabOrderService
    {
        /// <summary>Bác sĩ tạo chỉ định xét nghiệm / dịch vụ</summary>
        Task<LabOrderResponseDto> CreateLabOrderAsync(CreateLabOrderDto dto, int doctorId);

        /// <summary>Lấy danh sách chỉ định theo lịch hẹn</summary>
        Task<List<LabOrderResponseDto>> GetByAppointmentIdAsync(int appointmentId);

        /// <summary>Receptionist lấy danh sách chỉ định Pending chưa có hóa đơn</summary>
        Task<List<LabOrderResponseDto>> GetPendingLabOrdersAsync();

        /// <summary>Lấy danh sách dịch vụ y tế đang hoạt động (cho bác sĩ chọn)</summary>
        Task<List<MedicalServiceDto>> GetAllMedicalServicesAsync();
    }
}
