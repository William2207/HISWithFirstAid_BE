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

        public async Task<(List<Speciality> Items, int TotalCount)> GetPagedAsync(
            int page, int pageSize, string? searchQuery)
        {
            var query = _context.Specialties
                .Include(s => s.Doctors)
                .Include(s => s.HeadDoctor).ThenInclude(d => d.User)
                .Include(s => s.HeadNurse).ThenInclude(n => n.User)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var lowerSearch = searchQuery.ToLower();
                query = query.Where(s =>
                    s.Name.ToLower().Contains(lowerSearch) ||
                    (s.Description != null && s.Description.ToLower().Contains(lowerSearch)));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(s => s.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Speciality?> GetByIdAsync(int id)
        {
            return await _context.Specialties
                .Include(s => s.Doctors)
                .Include(s => s.HeadDoctor).ThenInclude(d => d.User)
                .Include(s => s.HeadNurse).ThenInclude(n => n.User)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Speciality> AddAsync(Speciality specialty)
        {
            _context.Specialties.Add(specialty);
            await _context.SaveChangesAsync();
            return specialty;
        }

        public async Task UpdateAsync(Speciality specialty)
        {
            _context.Specialties.Update(specialty);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Speciality specialty)
        {
            specialty.IsActive = false;
            _context.Specialties.Update(specialty);
            await _context.SaveChangesAsync();
        }
    }
}
