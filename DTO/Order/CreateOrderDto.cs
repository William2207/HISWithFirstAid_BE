using FirstAidAPI.Enums;

namespace FirstAidAPI.DTO.Order
{
    public class CreateOrderDto
    {
        public int UserId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public List<CreateOrderItemDto> Items { get; set; } = new List<CreateOrderItemDto>();
    }
}
