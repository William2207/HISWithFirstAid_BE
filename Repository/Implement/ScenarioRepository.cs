using FirstAidAPI.Data;
using FirstAidAPI.DTO;
using FirstAidAPI.Extensions;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstAidAPI.Repository.Implement
{
    public class ScenarioRepository : IScenarioRepository
    {
        private readonly FirstAidContext _context;

        public ScenarioRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Scenario>> GetAllAsync()
        {
            // Khi lấy danh sách, thường không cần tải các collection con để tránh dữ liệu quá lớn
            return await _context.Scenarios.ToListAsync();
        }

        public async Task<PagedResult<Scenario>> GetAllFilteredAndPagedAsync(int page, int pageSize, List<string>? difficulties, List<string>? types, string? search)
        {
            // Chuyển toàn bộ logic query từ Controller vào đây
            var query = _context.Scenarios.AsQueryable();

            // Filter theo difficulties
            if (difficulties != null && difficulties.Any())
            {
                query = query.Where(s => difficulties.Contains(s.Difficulty));
            }
            // Filter theo types
            if (types != null && types.Any())
            {
                query = query.Where(s => types.Contains(s.Type));
            }

            // Filter theo search
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(s =>
                    s.Title.ToLower().Contains(searchLower) ||
                    s.Description.ToLower().Contains(searchLower)
                );
            }

            query = query.OrderBy(s => s.Id);

            return await query.ToPagedResultAsync(page, pageSize);
        }

        public async Task<Scenario?> GetByIdAsync(int id)
        {
            return await _context.Scenarios
            .Include(s => s.ScenarioTechniques)
                .ThenInclude(st => st.Technique)
            .Include(s => s.ScenarioSteps.OrderBy(step => step.Order))
                .ThenInclude(ss => ss.Options) // Thêm ThenInclude để tải Options
            .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddAsync(Scenario scenario)
        {
            await _context.Scenarios.AddAsync(scenario);
        }

        public void Update(Scenario scenario)
        {
            _context.Scenarios.Update(scenario);
        }

        public void Delete(Scenario scenario)
        {
            _context.Scenarios.Remove(scenario);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}