using FirstAidAPI.DTO.Specialty;

namespace FirstAidAPI.Service
{
    public interface ISpecialtyService
    {
        Task<List<SpecialtyLookupDTO>> GetSpecialtiesForLookupAsync();
    }
}
