using FirstAidAPI.DTO;
using FirstAidAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstAidAPI.Service
{
    public interface IScenarioService
    {
        Task<IEnumerable<Scenario>> GetAllScenariosAsync();

        Task<PagedResult<Scenario>> GetScenariosAsync(int page, int pageSize, List<string>? difficulties, List<string>? types, string? search);

        Task<Scenario?> GetScenarioByIdAsync(int id);

        Task<Scenario> CreateScenarioAsync(Scenario scenario);

        Task<bool> UpdateScenarioAsync(int id, Scenario scenario);

        Task<bool> DeleteScenarioAsync(int id);
    }
}