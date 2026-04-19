using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IQueueRepository
    {
        Task<List<Queue>> GetAllAsync();

        Task<Queue> AddAsync(Queue queue);

        Task UpdateAsync(Queue queue);

        Task DeleteAsync(int id);

        Task<List<Queue>> GetByStatusAsync(string status);

        Task<int> GetNextQueueNumberAsync(DateOnly queueDate);

        Task<Queue?> GetNextWaitingQueueAsync();

        Task<Queue?> GetByIdAsync(int id);

        Task<Queue?> GetCurrentQueueAsync(DateOnly queueDate);

        Task<Queue?> IssueQueueAsync(DateOnly queueDate);
    }
}
