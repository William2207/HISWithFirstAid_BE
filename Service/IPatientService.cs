using FirstAidAPI.DTO.Patient;

namespace FirstAidAPI.Service
{
    public interface IPatientService
    {
        Task<PatientProfileDto?> GetPatientProfileAsync(int userId);
        Task<List<PatientProfileDto>> GetPatientsBySpecialtyAsync(int specialtyId);
        Task<bool> UpdatePatientProfileAsync(int userId, UpdatePatientProfileDto updateDto);
    }
}
