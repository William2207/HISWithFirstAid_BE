using FirstAidAPI.Data;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI.Repository.Implement

{
    public class ScenarioStepRepository : IScenarioStepRepository
    {
        private readonly FirstAidContext _context;

        public ScenarioStepRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<List<ScenarioStep>> GetByScenarioIdAsync(int scenarioId)
        {
            return await _context.ScenarioSteps
                .Where(step => step.ScenarioId == scenarioId)
                .OrderBy(step => step.Order)
                .ToListAsync();
        }

        public async Task AddAsync(ScenarioStep step)
        {
            await _context.ScenarioSteps.AddAsync(step);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ScenarioStep step)
        {
            _context.ScenarioSteps.Update(step);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int stepId)
        {
            var step = await _context.ScenarioSteps.FindAsync(stepId);
            if (step != null)
            {
                _context.ScenarioSteps.Remove(step);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteByScenarioIdAsync(int scenarioId)
        {
            var steps = _context.ScenarioSteps.Where(step => step.ScenarioId == scenarioId);
            _context.ScenarioSteps.RemoveRange(steps);
            await _context.SaveChangesAsync();
        }
    }
}