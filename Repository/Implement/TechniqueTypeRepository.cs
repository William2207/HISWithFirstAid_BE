using FirstAidAPI.Data;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI.Repository.Implement
{
    public class TechniqueTypeRepository : ITechniqueTypeRepository
    {
        private readonly FirstAidContext _firstAidContext;

        public TechniqueTypeRepository(FirstAidContext firstAidContext)
        {
            _firstAidContext = firstAidContext;
        }

        public async Task<IEnumerable<TechniqueType>> GetAllAsync()
        {
            return await _firstAidContext.TechniqueTypes
                .OrderBy(t => t.Id)
                .Include(t => t.Techniques)
                .ToListAsync();
        }

        public async Task<TechniqueType?> GetByIdAsync(int id)
        {
            return await _firstAidContext.TechniqueTypes
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TechniqueType> CreateAsync(TechniqueType techniqueType)
        {
            _firstAidContext.TechniqueTypes.Add(techniqueType);
            await _firstAidContext.SaveChangesAsync();
            return techniqueType;
        }

        public async Task<TechniqueType> UpdateAsync(TechniqueType techniqueType)
        {
            _firstAidContext.TechniqueTypes.Update(techniqueType);
            await _firstAidContext.SaveChangesAsync();
            return techniqueType;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var techniqueType = await GetByIdAsync(id);
            if (techniqueType == null)
                return false;

            _firstAidContext.TechniqueTypes.Remove(techniqueType);
            await _firstAidContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _firstAidContext.TechniqueTypes.AnyAsync(t => t.Id == id);
        }

        public async Task<bool> NameExistsAsync(string name, int? excludeId = null)
        {
            return await _firstAidContext.TechniqueTypes
                .AnyAsync(t => t.Name.ToLower() == name.ToLower() &&
                              (excludeId == null || t.Id != excludeId));
        }

        public async Task<bool> HasRelatedTechniquesAsync(int techniqueTypeId)
        {
            return await _firstAidContext.Techniques
                .AnyAsync(t => t.TechniqueTypeId == techniqueTypeId);
        }
    }
}