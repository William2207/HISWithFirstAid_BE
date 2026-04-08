using FirstAidAPI.DTO.Doctor;

namespace FirstAidAPI.Service
{
    public interface IDoctorService
    {
        Task<List<DoctorLookupDTO>> GetDoctorsForLookupAsync();
        Task<List<DoctorAvailabilityDTO>> GetAvailableDoctorsAsync(int specialtyId, DateTime date);
    }
}
