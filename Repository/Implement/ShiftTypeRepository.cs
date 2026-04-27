using FirstAidAPI.Data;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI.Repository.Implement
{
    public class ShiftTypeRepository : IShiftTypeRepository
    {
        private readonly FirstAidContext _context;

        public ShiftTypeRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<List<ShiftType>> GetAllAsync()
        {
            return await _context.ShiftTypes
                .Where(s => s.IsActive)
                .ToListAsync();
        }

        public async Task<ShiftType?> GetByIdAsync(int id)
        {
            return await _context.ShiftTypes.FindAsync(id);
        }

        public async Task<ShiftType?> GetNightShiftAsync()
        {
            return await _context.ShiftTypes
                .FirstOrDefaultAsync(s => s.IsNightShift && s.IsActive);
        }
    }
}
