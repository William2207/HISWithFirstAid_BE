using FirstAidAPI.Data;
using FirstAidAPI.DTO.Queue;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FirstAidAPI.Repository.Implement

{
    public class QueueRepository : IQueueRepository
    {
        private readonly FirstAidContext _context;

        public QueueRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<List<Queue>> GetAllAsync()
        {
            return await _context.Queues.ToListAsync();
        }

        public async Task<Queue> AddAsync(Queue queue)
        {
            _context.Queues.Add(queue);
            await _context.SaveChangesAsync();
            return queue;
        }

        public async Task DeleteAsync(int id)
        {
            var queue = await _context.Queues.FindAsync(id);
            if (queue != null)
            {
                _context.Queues.Remove(queue);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Queue queue)
        {
            _context.Queues.Update(queue);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Queue>> GetByStatusAsync(string status)
        {
            return await _context.Queues.Where(q => q.Status == status).ToListAsync();
        }

        public async Task<Queue> GetNextQueueNumberAsync(DateOnly queueDate)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
            try
            {
                var lastQueue = await _context.Queues
                    .Where(q => q.QueueDate == queueDate)  // chỉ lấy trong ngày hôm nay
                    .OrderByDescending(q => q.QueueNumber)
                    .FirstOrDefaultAsync();

                int nextNumber = lastQueue != null ? lastQueue.QueueNumber + 1 : 1;

                var newQueue = new Queue
                {
                    QueueNumber = nextNumber,
                    QueueDate = queueDate,
                };
                _context.Queues.Add(newQueue);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return newQueue;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Queue?> GetNextWaitingQueueAsync()
        {
            return await _context.Queues
                .Where(q => q.Status == "WAITING")
                .OrderBy(q => q.IssueTime)
                .FirstOrDefaultAsync();
        }

        public async Task<Queue?> GetByIdAsync(int id)
        {
            return await _context.Queues.FindAsync(id);
        }
    }
}
