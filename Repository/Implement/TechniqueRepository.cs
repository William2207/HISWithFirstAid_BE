using FirstAidAPI.Data;
using FirstAidAPI.DTO;
using FirstAidAPI.Extensions;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstAidAPI.Repository.Implement
{
    public class TechniqueRepository : ITechniqueRepository
    {
        private readonly FirstAidContext _context;

        public TechniqueRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Technique>> GetAllAsync()
        {
            // Sử dụng Include để tải các danh sách liên quan
            return await _context.Techniques
                .Include(t => t.ScenarioTechniques)
                .Include(t => t.TechniqueSteps)
                .ToListAsync();
        }

        public async Task<PagedResult<Technique>> GetAllFilteredAndPagedAsync(int page, int pageSize, List<string>? difficulties, List<string>? types, string? search)
        {
            var query = _context.Techniques.AsQueryable();

            // Filter theo difficulties
            if (difficulties != null && difficulties.Any())
            {
                query = query.Where(t => difficulties.Contains(t.Difficulty));
            }

            // Filter theo types
            if (types != null && types.Any())
            {
                query = query.Where(t => types.Contains(t.Type));
            }

            // Filter theo search
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(t =>
                    t.Title.ToLower().Contains(searchLower) ||
                    t.Description.ToLower().Contains(searchLower)
                );
            }

            query = query.OrderBy(t => t.Id);

            return await query.ToPagedResultAsync(page, pageSize);
        }

        public async Task<Technique?> GetByIdAsync(int id)
        {
            // Sử dụng Include và ThenInclude để tải các thực thể liên quan
            return await _context.Techniques
            .Include(t => t.TechniqueSteps.OrderBy(s => s.StepNumber))
            .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(Technique technique)
        {
            await _context.Techniques.AddAsync(technique);
        }

        public void Update(Technique technique)
        {
            _context.Techniques.Update(technique);
        }

        public void Delete(Technique technique)
        {
            _context.Techniques.Remove(technique);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}