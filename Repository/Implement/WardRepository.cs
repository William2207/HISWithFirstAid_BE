using FirstAidAPI.Data;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI.Repository.Implement
{
    public class WardRepository : IWardRepository
    {
        private readonly FirstAidContext _context;

        public WardRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<List<Ward>> GetBySpecialtyAsync(int specialtyId)
        {
            return await _context.Wards
                .Where(w => w.SpecialityId == specialtyId)
                .ToListAsync();
        }
    }
}
