using FirstAidAPI.DTO;
using FirstAidAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstAidAPI.Service
{
    public interface ITechniqueService
    {
        Task<IEnumerable<Technique>> GetAllTechniquesAsync();

        Task<PagedResult<Technique>> GetTechniquesAsync(int page, int pageSize, List<string>? difficulties, List<string>? types, string? search);

        Task<Technique?> GetTechniqueByIdAsync(int id);

        Task<Technique> CreateTechniqueAsync(Technique technique);

        Task<bool> UpdateTechniqueAsync(int id, Technique technique);

        Task<bool> DeleteTechniqueAsync(int id);
    }
}