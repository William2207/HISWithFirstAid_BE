using FirstAidAPI.DTO.Doctor;

namespace FirstAidAPI.Service
{
    public interface IDoctorService
    {
        Task<List<DoctorLookupDTO>> GetDoctorsForLookupAsync();
    }
}
