using FirstAidAPI.DTO.Order;

namespace FirstAidAPI.Service
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();

        Task<OrderDto> GetOrderByIdAsync(int orderId);

        Task<IEnumerable<OrderDto>> GetByUserIdAsync(int userId);

        Task<CreateOrderResponseDto> CreateOrderAsync(CreateOrderDto createOrderDto);

        Task<OrderDto?> GetByOrderNumberAsync(string orderNumber);

        Task CompleteOrderAsync(int orderId, string transactionId);

        Task FailOrderAsync(int orderId);
    }
}