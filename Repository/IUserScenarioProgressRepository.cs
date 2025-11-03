using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IUserScenarioProgressRepository
    {
        Task<UserScenarioProgress?> GetByUserIdAndScenarioIdAsync(int userId, int scenarioId);

        Task AddAsync(UserScenarioProgress progress);

        void Update(UserScenarioProgress progress);

        Task<bool> SaveChangesAsync();
    }
}