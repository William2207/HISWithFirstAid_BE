using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IUserTechniqueProgressRepository
    {
        Task<UserTechniqueProgress?> GetByUserIdAndTechniqueIdAsync(int userId, int techniqueId);

        Task AddAsync(UserTechniqueProgress progress);
        void Update(UserTechniqueProgress progress);
        Task<bool> SaveChangesAsync();
    }
}