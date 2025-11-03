using FirstAidAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using FirstAidAPI.DTO;

namespace FirstAidAPI.Repository
{
    public interface ITechniqueRepository
    {
        Task<IEnumerable<Technique>> GetAllAsync();

        Task<Technique?> GetByIdAsync(int id);

        Task AddAsync(Technique technique);

        void Update(Technique technique);

        void Delete(Technique technique);

        Task<bool> SaveChangesAsync();

        Task<PagedResult<Technique>> GetAllFilteredAndPagedAsync(int page, int pageSize, List<string>? difficulties, List<string>? types, string? search);
    }
}