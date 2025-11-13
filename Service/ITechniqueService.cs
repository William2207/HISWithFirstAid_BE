using FirstAidAPI.DTO;
using FirstAidAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstAidAPI.Service
{
    public interface ITechniqueService
    {
        Task<IEnumerable<Technique>> GetAllTechniquesAsync();

        Task<PagedResult<Technique>> GetTechniquesAsync(int page, int pageSize, List<string>? difficulties, List<int>? typeIds, string? search);

        Task<Technique?> GetTechniqueByIdAsync(int id);

        Task<TechniqueDto> CreateAsync(CreateTechniqueDto dto);

        Task<TechniqueDto?> UpdateAsync(int id, UpdateTechniqueDto dto);

        Task<List<TechniqueStepDto>> UpdateStepsAsync(int techniqueId, UpdateTechniqueStepsDto dto);

        Task<bool> DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);
    }
}