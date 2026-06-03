using FirstAidAPI.Data;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI.Repository.Implement
{
    public class BedRepository : IBedRepository
    {
        private readonly FirstAidContext _context;

        public BedRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<List<Bed>> GetAvailableBedsAsync()
        {
            return await _context.Beds
                .Include(b => b.Ward)
                .Where(b => b.Status == "AVAILABLE")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Bed?> GetByIdAsync(int id)
        {
            return await _context.Beds
                .Include(b => b.Ward)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task UpdateAsync(Bed bed)
        {
            _context.Beds.Update(bed);
            await _context.SaveChangesAsync();
        }
    }
}
