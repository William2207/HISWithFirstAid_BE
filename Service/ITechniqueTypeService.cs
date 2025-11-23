using FirstAidAPI.DTO.Technique;

namespace FirstAidAPI.Service
{
    public interface ITechniqueTypeService
    {
        Task<IEnumerable<TechniqueTypeDto>> GetAllAsync();

        Task<TechniqueTypeDto?> GetByIdAsync(int id);

        Task<TechniqueTypeDto> CreateAsync(CreateTechniqueTypeDto dto);

        Task<TechniqueTypeDto> UpdateAsync(int id, UpdateTechniqueTypeDto dto);

        Task<bool> DeleteAsync(int id);
    }
}