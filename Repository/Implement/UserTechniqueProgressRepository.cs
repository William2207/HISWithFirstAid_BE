using FirstAidAPI.Data;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FirstAidAPI.Repository.Implement
{
    public class UserTechniqueProgressRepository : IUserTechniqueProgressRepository
    {
        private readonly FirstAidContext _context;

        public UserTechniqueProgressRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<UserTechniqueProgress?> GetByUserIdAndTechniqueIdAsync(int userId, int techniqueId)
        {
            return await _context.UserTechniqueProgresses
                .FirstOrDefaultAsync(p => p.UserId == userId && p.TechniqueId == techniqueId);
        }

        public async Task AddAsync(UserTechniqueProgress progress)
        {
            await _context.UserTechniqueProgresses.AddAsync(progress);
        }

        public void Update(UserTechniqueProgress progress)
        {
            _context.UserTechniqueProgresses.Update(progress);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}