using FirstAidAPI.Data;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI.Repository.Implement
{
    public class SpecialtyRepository : ISpecialtyRepository
    {
        private readonly FirstAidContext _context;

        public SpecialtyRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<List<Speciality>> GetAllActiveAsync()
        {
            return await _context.Specialties
                .Where(s => s.IsActive)
                .ToListAsync();
        }

        public async Task<Speciality?> GetByIdAsync(int id)
        {
            return await _context.Specialties
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
