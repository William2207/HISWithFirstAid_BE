using FirstAidAPI.DTO;
using FirstAidAPI.DTO.Scenario;
using FirstAidAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstAidAPI.Repository
{
    public interface IScenarioRepository
    {
        Task<IEnumerable<Scenario>> GetAllAsync();

        Task<PagedResult<ScenarioDto>> GetAllFilteredAndPagedAsync(int page, int pageSize, List<string>? difficulties, List<string>? types, string? search);

        Task<Scenario?> GetByIdAsync(int id);

        Task<Scenario> CreateAsync(Scenario scenario);

        Task<Scenario> UpdateAsync(Scenario scenario);

        Task<bool> DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);
    }
}