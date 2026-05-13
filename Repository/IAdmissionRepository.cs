using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IAdmissionRepository
    {
        Task<AdmissionRecord> CreateAsync(AdmissionRecord record);
        Task<AdmissionRecord?> GetActiveAdmissionByPatientIdAsync(int patientId);
        Task UpdateAsync(AdmissionRecord record);
    }
}
