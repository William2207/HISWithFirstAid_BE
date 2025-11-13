using FirstAidAPI.Data;
using FirstAidAPI.DTO;
using FirstAidAPI.Extensions;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstAidAPI.Repository.Implement
{
    public class TechniqueRepository : ITechniqueRepository
    {
        private readonly FirstAidContext _context;
        private IDbContextTransaction? _currentTransaction;

        public TechniqueRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Technique>> GetAllAsync()
        {
            // Sử dụng Include để tải các danh sách liên quan
            return await _context.Techniques
                .Include(t => t.Type) // Thêm dòng này
                .Include(t => t.ScenarioTechniques)
                .Include(t => t.TechniqueSteps)
                .OrderBy(t => t.Id) // Nên thêm ordering
                .ToListAsync();
        }

        public async Task<PagedResult<Technique>> GetAllFilteredAndPagedAsync(int page, int pageSize, List<string>? difficulties, List<int>? typeIds, string? search)
        {
            var query = _context.Techniques
                .AsNoTracking()
                .Include(t => t.Type) // Include để có thể access Type.Name
                .AsQueryable();

            // Filter theo difficulties
            if (difficulties != null && difficulties.Any())
            {
                query = query.Where(t => difficulties.Contains(t.Difficulty));
            }

            // Filter theo types
            if (typeIds != null && typeIds.Any())
            {
                query = query.Where(t => typeIds.Contains(t.TechniqueTypeId));
            }

            // Filter theo search
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(t =>
                    t.Title.ToLower().Contains(searchLower) ||
                    t.Description.ToLower().Contains(searchLower)
                );
            }

            query = query.OrderBy(t => t.Id);

            return await query.ToPagedResultAsync(page, pageSize);
        }

        public async Task<Technique?> GetByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            return await _context.Techniques
                .Include(t => t.Type)
                .Include(t => t.TechniqueSteps.OrderBy(s => s.StepNumber))
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Technique?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Techniques
                .Include(t => t.Type)
                .Include(t => t.QuizQuestions)
                    .ThenInclude(q => q.AnswerOptions)
                .Include(t => t.TechniqueSteps)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Technique> AddAsync(Technique technique)
        {
            _context.Techniques.Add(technique);
            await _context.SaveChangesAsync();

            // Load the Type navigation property
            await _context.Entry(technique).Reference(t => t.Type).LoadAsync();

            return technique;
        }

        public async Task<Technique> UpdateAsync(Technique technique)
        {
            _context.Techniques.Update(technique);
            await _context.SaveChangesAsync();

            // Reload the Type navigation property
            await _context.Entry(technique).Reference(t => t.Type).LoadAsync();

            return technique;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var technique = await _context.Techniques.FindAsync(id);
            if (technique == null)
                return false;

            _context.Techniques.Remove(technique);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Techniques.AnyAsync(t => t.Id == id);
        }

        public async Task<bool> TechniqueTypeExistsAsync(int techniqueTypeId)
        {
            return await _context.TechniqueTypes.AnyAsync(tt => tt.Id == techniqueTypeId);
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