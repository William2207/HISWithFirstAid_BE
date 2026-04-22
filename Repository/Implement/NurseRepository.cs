using FirstAidAPI.Data;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI.Repository.Implement
{
    public class NurseRepository : INurseRepository
    {
        private readonly FirstAidContext _context;

        public NurseRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<Nurse> AddAsync(Nurse nurse)
        {
            await _context.Nurses.AddAsync(nurse);
            await _context.SaveChangesAsync();
            return nurse;
        }

        public async Task UpdateAsync(Nurse nurse)
        {
            _context.Nurses.Update(nurse);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var nurse = await _context.Nurses.FindAsync(id);
            if (nurse != null)
            {
                _context.Nurses.Remove(nurse);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Nurse?> GetByIdAsync(int id)
        {
            return await _context.Nurses
                .Include(n => n.User)
                .Include(n => n.Speciality)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<Nurse?> GetByUserIdAsync(int userId)
        {
            return await _context.Nurses
                .Include(n => n.User)
                .Include(n => n.Speciality)
                .FirstOrDefaultAsync(n => n.UserId == userId);
        }

        public async Task<List<Nurse>> GetAllAsync()
        {
            return await _context.Nurses
                .Include(n => n.User)
                .Include(n => n.Speciality)
                .ToListAsync();
        }

        public async Task<bool> ExistsByUserIdAsync(int userId)
        {
            return await _context.Nurses
                .AnyAsync(n => n.UserId == userId);
        }
    }
}
