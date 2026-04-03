using FirstAidAPI.Data;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI.Repository.Implement
{
    public class MedicalServiceRepository : IMedicalServiceRepository
    {
        private readonly FirstAidContext _context;

        public MedicalServiceRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<List<MedicalService>> GetAllActiveAsync()
        {
            return await _context.MedicalServices
                .Where(s => s.IsActive)
                .ToListAsync();
        }

        public async Task<MedicalService?> GetByIdAsync(int id)
        {
            return await _context.MedicalServices
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
