using FirstAidAPI.DTO;
using FirstAidAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstAidAPI.Service
{
    public interface IScenarioService
    {
        Task<IEnumerable<ScenarioDto>> GetAllScenariosAsync();

        Task<PagedResult<Scenario>> GetScenariosAsync(int page, int pageSize, List<string>? difficulties, List<string>? types, string? search);

        Task<Scenario?> GetScenarioByIdAsync(int id);

        Task<ScenarioDetailDto> CreateScenarioAsync(CreateScenarioDto createDto);

        Task<ScenarioDetailDto> UpdateScenarioAsync(int id, UpdateScenarioDto updateDto);

        Task<bool> DeleteScenarioAsync(int id);
    }
}