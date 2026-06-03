using FirstAidAPI.DTO.Ward;
using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IWardRepository
    {
        // ─── Wards ───────────────────────────────────────────────────────────
        Task<List<Ward>> GetBySpecialtyAsync(int specialtyId);

        // ─── Admissions by Room ──────────────────────────────────────────────
        Task<List<AdmissionRecord>> GetActiveAdmissionsByRoomAsync(string roomNumber);

        // ─── Room Overview ───────────────────────────────────────────────────
        Task<List<WardRoomSummaryDto>> GetRoomSummariesAsync(List<int>? allowedWardIds = null);
        Task<List<int>> GetAssignedWardIdsForDoctorAsync(int doctorId, DateOnly date);
        Task<List<int>> GetAssignedWardIdsForNurseAsync(int nurseId, DateOnly date);

        // ─── Ward Orders ─────────────────────────────────────────────────────
        Task<WardOrder?> GetOrderByIdAsync(int id);
        Task<List<WardOrder>> GetOrdersByAdmissionAsync(int admissionRecordId);
        Task<WardOrder> CreateOrderAsync(WardOrder order);
        Task UpdateOrderAsync(WardOrder order);

        // ─── Ward Notes ──────────────────────────────────────────────────────
        Task<List<WardNote>> GetNotesByAdmissionAsync(int admissionRecordId);
        Task<WardNote> CreateNoteAsync(WardNote note);

        // ─── Vital Signs ─────────────────────────────────────────────────────
        Task<List<VitalSign>> GetVitalsHistoryByAdmissionAsync(int admissionRecordId);
        Task<VitalSign> CreateVitalSignAsync(VitalSign vitalSign);
    }
}
