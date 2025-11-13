using FirstAidAPI.DTO;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace FirstAidAPI.Repository
{
    public interface ITechniqueStepRepository
    {
        Task<TechniqueStep> AddAsync(TechniqueStep techniqueStep);

        Task<TechniqueStep> UpdateAsync(TechniqueStep techniqueStep);

        Task<bool> DeleteAsync(int id);

        Task<IDbContextTransaction> BeginTransactionAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();

        Task<TechniqueStep?> GetByIdAsync(int id);
    }
}