using FirstAidAPI.Data;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI.Repository.Implement
{
    public class ClinicRepository : IClinicRepository
    {
        private readonly FirstAidContext _context;

        public ClinicRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<List<Clinic>> GetAllAsync()
        {
            return await _context.Clinics
                .Include(c => c.Specialty)
                .ToListAsync();
        }

        public async Task<Clinic?> GetByIdAsync(int id)
        {
            return await _context.Clinics
                .Include(c => c.Specialty)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Clinic>> GetBySpecialtyAsync(int specialtyId)
        {
            return await _context.Clinics
                .Where(c => c.SpecialtyId == specialtyId)
                .ToListAsync();
        }
    }
}
