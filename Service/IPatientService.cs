using FirstAidAPI.DTO.Patient;

namespace FirstAidAPI.Service
{
    public interface IPatientService
    {
        Task<PatientProfileDto?> GetPatientProfileAsync(int userId);
        Task<bool> UpdatePatientProfileAsync(int userId, UpdatePatientProfileDto updateDto);
    }
}
