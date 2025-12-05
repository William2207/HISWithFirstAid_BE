using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();

        Task<Order?> GetByIdAsync(int id);

        Task<Order> CreateAsync(Order order);

        Task<Order> UpdateAsync(Order order);

        Task<bool> ExistAsync(int id);

        Task<Order?> GetByOrderNumberAsync(string orderNumber);

        Task<IEnumerable<Order>> GetByUserIdAsync(int userId);

        Task<Order?> GetByIdWithItemsAsync(int id);
    }
}