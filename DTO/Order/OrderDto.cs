using FirstAidAPI.DTO.User;
using FirstAidAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace FirstAidAPI.DTO.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }

        [EnumDataType(typeof(PaymentMethod))]
        public PaymentMethod PaymentMethod { get; set; }

        [EnumDataType(typeof(PaymentStatus))]
        public PaymentStatus PaymentStatus { get; set; }

        [EnumDataType(typeof(OrderStatus))]
        public OrderStatus OrderStatus { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? CompleteAt { get; set; }
        public string? TransactionId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty; // Chỉ lấy info cần thiết
        public string UserEmail { get; set; } = string.Empty;

        public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }
}