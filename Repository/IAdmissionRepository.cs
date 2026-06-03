using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IAdmissionRepository
    {
        Task<AdmissionRecord> CreateAsync(AdmissionRecord record);
        Task<AdmissionRecord?> GetActiveAdmissionByPatientIdAsync(int patientId);
        Task<List<AdmissionRecord>> GetActiveAdmissionsAsync();
        Task<List<AdmissionRecord>> GetAllByPatientIdAsync(int patientId);
        Task UpdateAsync(AdmissionRecord record);
    }
}
