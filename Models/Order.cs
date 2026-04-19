using System.ComponentModel.DataAnnotations.Schema;
using FirstAidAPI.Enums;

namespace FirstAidAPI.Models
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }
        public string OrderNumber { get; set; } = string.Empty; // Mã đơn hàng duy nhất (VD: ORD20250129001)
        public decimal TotalAmount { get; set; } // Tổng tiền
        public PaymentMethod PaymentMethod { get; set; }// Phương thức thanh toán
        public PaymentStatus PaymentStatus { get; set; }// Trạng thái: Pending, Completed, Failed
        public OrderStatus OrderStatus { get; set; } // Trạng thái đơn: Processing, Completed, Cancelled
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; } // Thời điểm hoàn thành
        public string? TransactionId { get; set; }

        // Navigation properties
        public User? User { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
