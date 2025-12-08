using FirstAidAPI.Data;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI.Repository.Implement
{
    public class SavedTechniqueRepository : ISavedTechniqueRepository
    {
        private readonly FirstAidContext _context;

        public SavedTechniqueRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<SavedTechnique> AddSavedTechniqueAsync(SavedTechnique savedTechnique)
        {
            _context.SavedTechniques.Add(savedTechnique);
            await _context.SaveChangesAsync();
            return savedTechnique;
        }

        public async Task<bool> RemoveSavedTechniqueAsync(int userId, int techniqueId)
        {
            var savedTechnique = await _context.SavedTechniques
                .FirstOrDefaultAsync(st => st.UserId == userId && st.TechniqueId == techniqueId);
            if (savedTechnique == null)
            {
                return false;
            }
            _context.SavedTechniques.Remove(savedTechnique);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SavedTechnique>> GetSavedTechniquesByUserAsync(int userId)
        {
            return await _context.SavedTechniques
                .Include(st => st.Technique)
                .Where(st => st.UserId == userId)
                .ToListAsync();
        }
    }
}