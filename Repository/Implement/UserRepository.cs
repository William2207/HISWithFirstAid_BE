using FirstAidAPI.Data;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstAidAPI.Repository.Implement
{
    public class UserRepository : IUserRepository
    {
        private readonly FirstAidContext _context;

        public UserRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            // Khi lấy danh sách, không nên include các list con để tránh quá tải
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            // Khi lấy chi tiết một user, có thể include các thông tin liên quan nếu cần
            return await _context.Users
                .Include(u => u.ScenarioProgresses)
                .Include(u => u.TechniqueProgresses)
                .Include(u => u.SavedTechniques)
                .Include(u => u.ScenarioAttempts)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => EF.Functions.Like(u.Email, email));
        }

        public async Task<IEnumerable<SavedTechnique>> GetSavedTechniquesAsync(int userId)
        {
            return await _context.SavedTechniques
                .Include(st => st.Technique)
                .Where(st => st.UserId == userId)
                .OrderByDescending(st => st.SavedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<ScenarioAttempt>> GetScenarioAttemptsAsync(int userId, int limit)
        {
            return await _context.ScenarioAttempts
                .Include(sa => sa.Scenario)
                .Where(sa => sa.UserId == userId)
                .OrderByDescending(sa => sa.AttemptedAt)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserScenarioProgress>> GetScenarioProgressesAsync(int userId)
        {
            return await _context.UserScenarioProgresses
                .Include(sp => sp.Scenario)
                .Where(sp => sp.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserTechniqueProgress>> GetTechniqueProgressesAsync(int userId)
        {
            return await _context.UserTechniqueProgresses
                .Include(tp => tp.Technique)
                .Where(tp => tp.UserId == userId)
                .ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}