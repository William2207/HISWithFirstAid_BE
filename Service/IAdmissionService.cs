using FirstAidAPI.DTO.Admission;

namespace FirstAidAPI.Service
{
    public interface IAdmissionService
    {
        /// <summary>Danh sách bệnh nhân được bác sĩ chỉ định nhập viện nhưng chưa có giường.</summary>
        Task<List<PendingAdmissionDto>> GetPendingAdmissionsAsync();

        /// <summary>Danh sách giường đang trống để y tá chọn.</summary>
        Task<List<BedDto>> GetAvailableBedsAsync();

        /// <summary>Gán giường cho bệnh nhân và tạo AdmissionRecord. Trả về record vừa tạo.</summary>
        Task<AdmissionRecordDto> AssignBedAsync(int nurseUserId, AssignBedRequest request);

        /// <summary>Danh sách bệnh nhân đang nằm viện.</summary>
        Task<List<AdmissionRecordDto>> GetActiveAdmissionsAsync();

        /// <summary>Lịch sử tất cả lần nhập viện của một bệnh nhân (kể cả đã xuất viện).</summary>
        Task<List<AdmissionRecordDto>> GetAdmissionHistoryByPatientIdAsync(int patientId);

        /// <summary>Xuất viện: giải phóng giường và cập nhật DischargedAt.</summary>
        Task DischargePatientAsync(int patientId);
    }
}
