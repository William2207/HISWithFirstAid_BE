using FirstAidAPI.DTO;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstAidAPI.Repository
{
    public interface ITechniqueRepository
    {
        Task<IEnumerable<Technique>> GetAllAsync();

        Task<Technique?> GetByIdAsync(int id);

        Task<Technique?> GetByIdWithDetailsAsync(int id);

        Task<Technique> AddAsync(Technique technique);

        Task<Technique> UpdateAsync(Technique technique);

        Task<bool> DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task<bool> TechniqueTypeExistsAsync(int techniqueTypeId);

        Task<IDbContextTransaction> BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();

        Task<PagedResult<Technique>> GetAllFilteredAndPagedAsync(int page, int pageSize, List<string>? difficulties, List<int>? typeIds, string? search);
    }
}