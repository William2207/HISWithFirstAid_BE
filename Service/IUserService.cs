using FirstAidAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstAidAPI.Service
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();

        Task<User?> GetUserByIdAsync(int id);

        Task<User?> GetUserByEmailAsync(string email);

        Task<User> CreateUserAsync(User user);

        Task<bool> UpdateUserAsync(int id, User user);

        Task<bool> ChangePasswordAsync(User user, string currentPassword, string newPassword);

        Task<bool> DeleteUserAsync(int id);

        Task<IEnumerable<SavedTechnique>> GetSavedTechniquesByUserIdAsync(int userId);

        Task<IEnumerable<ScenarioAttempt>> GetScenarioAttemptsByUserIdAsync(int userId, int limit);

        Task<IEnumerable<UserScenarioProgress>> GetScenarioProgressesByUserIdAsync(int userId);

        Task<IEnumerable<UserTechniqueProgress>> GetTechniqueProgressesByUserIdAsync(int userId);
    }
}
