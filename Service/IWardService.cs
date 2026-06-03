using FirstAidAPI.DTO.Ward;

namespace FirstAidAPI.Service
{
    public interface IWardService
    {
        /// <summary>Tóm tắt tất cả các phòng: số giường, số bệnh nhân.</summary>
        Task<List<WardRoomSummaryDto>> GetRoomSummariesAsync(int userId, string role);

        /// <summary>Danh sách bệnh nhân nội trú trong một phòng cụ thể.</summary>
        Task<List<WardPatientDto>> GetPatientsByRoomAsync(string roomNumber);

        // ─── Orders ──────────────────────────────────────────────────────────
        Task<List<WardOrderDto>> GetOrdersByAdmissionAsync(int admissionRecordId);
        Task<WardOrderDto> CreateOrderAsync(int doctorUserId, CreateWardOrderRequest request);
        Task<WardOrderDto> UpdateOrderAsync(int orderId, int doctorUserId, UpdateWardOrderRequest request);
        Task<WardOrderDto> UpdateOrderStatusAsync(int orderId, string newStatus);

        // ─── Notes ───────────────────────────────────────────────────────────
        Task<List<WardNoteDto>> GetNotesByAdmissionAsync(int admissionRecordId);
        Task<WardNoteDto> CreateNoteAsync(int authorUserId, string authorRole, CreateWardNoteRequest request);

        // ─── Vital Signs ─────────────────────────────────────────────────────
        Task<List<VitalsDto>> GetVitalsHistoryByAdmissionAsync(int admissionRecordId);
        Task<VitalsDto> LogVitalsAsync(int? nurseUserId, int admissionRecordId, LogVitalsRequest request);
    }
}
