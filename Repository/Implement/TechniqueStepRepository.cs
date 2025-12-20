using FirstAidAPI.Data;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI.Repository.Implement
{
    public class TechniqueStepRepository : ITechniqueStepRepository
    {
        private readonly FirstAidContext _context;
        private IDbContextTransaction? _currentTransaction;

        public TechniqueStepRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<TechniqueStep> AddAsync(TechniqueStep techniqueStep)
        {
            _context.TechniqueSteps.Add(techniqueStep);
            await _context.SaveChangesAsync();
            return techniqueStep;
        }

        public async Task<TechniqueStep> UpdateAsync(TechniqueStep techniqueStep)
        {
            _context.TechniqueSteps.Update(techniqueStep);
            await _context.SaveChangesAsync();
            return techniqueStep;
        }

        public async Task<bool> DeleteAsync(int Id)
        {
            var existStep = await _context.TechniqueSteps.FindAsync(Id);
            if (existStep == null)
                return false;

            _context.TechniqueSteps.Remove(existStep);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TechniqueStep?> GetByIdAsync(int id)
        {
            if (id == 0)
                return null;
            return await _context.TechniqueSteps.
                FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }

            _currentTransaction = await _context.Database.BeginTransactionAsync();
            return _currentTransaction;
        }

        //Transaction
        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();

                if (_currentTransaction != null)
                {
                    await _currentTransaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.RollbackAsync();
                }
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}