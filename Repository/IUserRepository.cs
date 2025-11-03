using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();

        Task<User?> GetByIdAsync(int id);

        Task<User?> GetByEmailAsync(string email);

        Task AddAsync(User user);

        void Update(User user);

        void Delete(User user);

        Task<bool> SaveChangesAsync();

        Task<IEnumerable<SavedTechnique>> GetSavedTechniquesAsync(int userId);

        Task<IEnumerable<ScenarioAttempt>> GetScenarioAttemptsAsync(int userId, int limit);

        Task<IEnumerable<UserScenarioProgress>> GetScenarioProgressesAsync(int userId);

        Task<IEnumerable<UserTechniqueProgress>> GetTechniqueProgressesAsync(int userId);
    }
}