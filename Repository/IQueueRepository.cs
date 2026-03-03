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

        Task<Queue> GetNextQueueNumberAsync(DateOnly queueDate);

        Task<Queue?> GetNextWaitingQueueAsync();

        Task<Queue?> GetByIdAsync(int id);
    }
}
