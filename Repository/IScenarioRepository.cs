using FirstAidAPI.DTO;
using FirstAidAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstAidAPI.Repository
{
    public interface IScenarioRepository
    {
        Task<IEnumerable<Scenario>> GetAllAsync();

        Task<PagedResult<Scenario>> GetAllFilteredAndPagedAsync(int page, int pageSize, List<string>? difficulties, List<string>? types, string? search);

        Task<Scenario?> GetByIdAsync(int id);

        Task AddAsync(Scenario scenario);

        void Update(Scenario scenario);

        void Delete(Scenario scenario);

        Task<bool> SaveChangesAsync();
    }
}