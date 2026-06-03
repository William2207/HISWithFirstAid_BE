using FirstAidAPI.DTO.Specialty;

namespace FirstAidAPI.Service
{
    public interface ISpecialtyService
    {
        Task<List<SpecialtyLookupDTO>> GetSpecialtiesForLookupAsync();
        Task<(List<SpecialtyDTO> Items, int TotalCount)> GetPagedSpecialtiesAsync(int page, int pageSize, string? searchQuery);
        Task<SpecialtyDTO> GetSpecialtyByIdAsync(int id);
        Task<SpecialtyDTO> CreateSpecialtyAsync(CreateSpecialtyRequest request);
        Task<SpecialtyDTO> UpdateSpecialtyAsync(int id, UpdateSpecialtyRequest request);
        Task DeleteSpecialtyAsync(int id);
    }
}
