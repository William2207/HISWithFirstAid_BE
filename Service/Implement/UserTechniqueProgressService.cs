using FirstAidAPI.Models;
using FirstAidAPI.Repository;
using System;
using System.Threading.Tasks;

namespace FirstAidAPI.Service.Implement
{
    public class UserTechniqueProgressService : IUserTechniqueProgressService
    {
        private readonly IUserTechniqueProgressRepository _progressRepository;

        public UserTechniqueProgressService(IUserTechniqueProgressRepository progressRepository)
        {
            _progressRepository = progressRepository;
        }

        // Chữ ký phương thức này vẫn hoàn hảo, không phụ thuộc vào DTO
        public async Task<bool> SaveCompletionProgressAsync(int userId, int techniqueId)
        {
            var existingProgress = await _progressRepository.GetByUserIdAndTechniqueIdAsync(userId, techniqueId);

            if (existingProgress != null)
            {
                existingProgress.Status = true;
                existingProgress.CompletedAt = DateTime.UtcNow;
                existingProgress.LastAccessedAt = DateTime.UtcNow;
                _progressRepository.Update(existingProgress);
            }
            else
            {
                var newProgress = new UserTechniqueProgress
                {
                    UserId = userId,
                    TechniqueId = techniqueId,
                    Status = true,
                    StartedAt = DateTime.UtcNow,
                    CompletedAt = DateTime.UtcNow,
                    LastAccessedAt = DateTime.UtcNow
                };
                await _progressRepository.AddAsync(newProgress);
            }

            return await _progressRepository.SaveChangesAsync();
        }
    }
}
