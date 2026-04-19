using FirstAidAPI.Data;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI.Repository.Implement
{
    public class ReceptionistRepository : IReceptionistRepository
    {
        private readonly FirstAidContext _context;

        public ReceptionistRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<Receptionist> AddAsync(Receptionist receptionist)
        {
            await _context.Receptionists.AddAsync(receptionist);
            await _context.SaveChangesAsync();
            return receptionist;
        }

        public async Task UpdateAsync(Receptionist receptionist)
        {
            _context.Receptionists.Update(receptionist);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var receptionist = await _context.Receptionists.FindAsync(id);
            if (receptionist != null)
            {
                _context.Receptionists.Remove(receptionist);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Receptionist?> GetByIdAsync(int id)
        {
            return await _context.Receptionists
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Receptionist?> GetByUserIdAsync(int userId)
        {
            return await _context.Receptionists
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.UserId == userId);
        }

        public async Task<List<Receptionist>> GetAllAsync()
        {
            return await _context.Receptionists
                .Include(r => r.User)
                .ToListAsync();
        }

        public async Task<bool> ExistsByUserIdAsync(int userId)
        {
            return await _context.Receptionists
                .AnyAsync(r => r.UserId == userId);
        }
    }
}
