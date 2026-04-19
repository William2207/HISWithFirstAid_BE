using FirstAidAPI.DTO.Order;
using FirstAidAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace FirstAidAPI.DTO.Invoice
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;

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
        public string UserName { get; set; } = string.Empty;
    }
}
