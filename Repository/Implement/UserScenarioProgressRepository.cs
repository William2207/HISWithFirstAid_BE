using FirstAidAPI.Data;
using FirstAidAPI.Models;
using FirstAidAPI.DTO;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI.Repository.Implement
{
    public class UserScenarioProgressRepository : IUserScenarioProgressRepository
    {
        private readonly FirstAidContext _context;

        public UserScenarioProgressRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<UserScenarioProgress?> GetByUserIdAndScenarioIdAsync(int userId, int scenarioId)
        {
            return await _context.UserScenarioProgresses
                .FirstOrDefaultAsync(usp => usp.UserId == userId && usp.ScenarioId == scenarioId);
        }

        public async Task AddAsync(UserScenarioProgress progress)
        {
            await _context.UserScenarioProgresses.AddAsync(progress);
        }

        public void Update(UserScenarioProgress progress)
        {
            _context.UserScenarioProgresses.Update(progress);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}