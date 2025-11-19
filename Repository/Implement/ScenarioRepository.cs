using FirstAidAPI.Data;
using FirstAidAPI.DTO;
using FirstAidAPI.Extensions;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace FirstAidAPI.Repository.Implement
{
    public class ScenarioRepository : IScenarioRepository
    {
        private readonly FirstAidContext _context;
        private readonly IMapper _mapper;

        public ScenarioRepository(FirstAidContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Scenario>> GetAllAsync()
        {
            return await _context.Scenarios
                .Include(s => s.ScenarioTechniques)
                    .ThenInclude(st => st.Technique)
                .Include(s => s.ScenarioSteps)
                    .ThenInclude(ss => ss.Options)
                .Include(s => s.ScenarioSteps)
                    .ThenInclude(ss => ss.Technique)
                .OrderBy(s => s.Id)
                .ToListAsync();
        }

        public async Task<PagedResult<ScenarioDto>> GetAllFilteredAndPagedAsync(int page, int pageSize, List<string>? difficulties, List<string>? types, string? search)
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

            var dtoQuery = query.Select(s => new ScenarioDto
            {
                Id = s.Id,
                Name = s.Name,
                Title = s.Title,
                Description = s.Description,
                Type = s.Type,
                Difficulty = s.Difficulty,
                Duration = s.Duration,
                Icon = s.Icon!,
                PassingScore = s.PassingScore,
                IsPublished = s.IsPublished,
                StepCount = s.ScenarioSteps.Count
            });

            return await dtoQuery.ToPagedResultAsync(page, pageSize);
        }

        public async Task<ScenarioDetailDto?> GetByIdAsync(int id)
        {
            var scenario = await _context.Scenarios
               .Include(s => s.ScenarioTechniques)
                   .ThenInclude(st => st.Technique)
               .Include(s => s.ScenarioSteps)
                   .ThenInclude(ss => ss.Options)
               .FirstOrDefaultAsync(s => s.Id == id);

            return _mapper.Map<ScenarioDetailDto>(scenario);
        }

        public async Task<Scenario> CreateAsync(Scenario scenario)
        {
            _context.Scenarios.Add(scenario);
            await _context.SaveChangesAsync();
            return scenario;
        }

        public async Task<Scenario> UpdateAsync(Scenario scenario)
        {
            _context.Entry(scenario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return scenario;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var scenario = await _context.Scenarios.FindAsync(id);
            if (scenario == null)
            {
                return false;
            }

            _context.Scenarios.Remove(scenario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Scenarios.AnyAsync(s => s.Id == id);
        }
    }
}