using FirstAidAPI.DTO.Doctor;

namespace FirstAidAPI.Service
{
    public interface IDoctorService
    {
        Task<List<DoctorLookupDTO>> GetDoctorsForLookupAsync();

        Task<List<DoctorAvailabilityDTO>> GetAvailableDoctorsAsync(int specialtyId, DateTime date);

        Task<int> GetDoctorIdByUserId(int userId);

        Task<DoctorProfileDto?> GetDoctorProfileAsync(int userId);
        Task<bool> UpdateDoctorProfileAsync(int userId, UpdateDoctorProfileDto updateDto);
    }
}
