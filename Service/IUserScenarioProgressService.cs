using FirstAidAPI.DTO;

namespace FirstAidAPI.Service
{
    public interface IUserScenarioProgressService
    {
        Task<bool> SaveCompletionProgressAsync(int userId, int scenarioId, int score);
    }
}