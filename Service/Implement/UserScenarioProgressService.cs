using FirstAidAPI.Repository;
using FirstAidAPI.Models;

namespace FirstAidAPI.Service.Implement
{
    public class UserScenarioProgressService : IUserScenarioProgressService
    {
        private readonly IUserScenarioProgressRepository _userScenarioProgressRepository;

        public UserScenarioProgressService(IUserScenarioProgressRepository userScenarioProgressRepository)
        {
            _userScenarioProgressRepository = userScenarioProgressRepository;
        }

        public async Task<bool> SaveCompletionProgressAsync(int userId, int scenarioId, int score)
        {
            var existingProgress = await _userScenarioProgressRepository.GetByUserIdAndScenarioIdAsync(userId, scenarioId);
            if (existingProgress != null)
            {
                existingProgress.Status = true;
                existingProgress.HighestScore = Math.Max(existingProgress.HighestScore, score);
                existingProgress.CompletedAt = DateTime.UtcNow;
                existingProgress.LastAccessedAt = DateTime.UtcNow;
                _userScenarioProgressRepository.Update(existingProgress);
            }
            else
            {
                var newProgress = new UserScenarioProgress
                {
                    UserId = userId,
                    ScenarioId = scenarioId,
                    Status = true,
                    HighestScore = score,
                    StartedAt = DateTime.UtcNow,
                    CompletedAt = DateTime.UtcNow,
                    LastAccessedAt = DateTime.UtcNow
                };
                await _userScenarioProgressRepository.AddAsync(newProgress);
            }
            return await _userScenarioProgressRepository.SaveChangesAsync();
        }
    }
}