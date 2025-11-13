using FirstAidAPI.Models;
using System.Threading.Tasks;

namespace FirstAidAPI.Repository
{
    public interface ITechniqueTypeRepository
    {
        Task<IEnumerable<TechniqueType>> GetAllAsync();

        Task<TechniqueType?> GetByIdAsync(int id);

        Task<TechniqueType> CreateAsync(TechniqueType techniqueType);

        Task<TechniqueType> UpdateAsync(TechniqueType techniqueType);

        Task<bool> DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task<bool> NameExistsAsync(string name, int? excludeId = null);

        Task<bool> HasRelatedTechniquesAsync(int techniqueTypeId);
    }
}