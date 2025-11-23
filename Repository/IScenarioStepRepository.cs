using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IScenarioStepRepository
    {
        Task<List<ScenarioStep>> GetByScenarioIdAsync(int scenarioId);

        Task AddAsync(ScenarioStep step);

        Task UpdateAsync(ScenarioStep step);

        Task DeleteAsync(int stepId);

        Task DeleteByScenarioIdAsync(int scenarioId);
    }
}