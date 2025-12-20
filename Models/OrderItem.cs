using System.ComponentModel.DataAnnotations.Schema;

namespace FirstAidAPI.Models
{
    public class OrderItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int OrderId { get; set; }
        public int PracticalCourseId { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal Price { get; set; } // Giá tại thời điểm mua
        public decimal Subtotal { get; set; } // = Price * Quantity

        // Navigation properties
        public Order? Order { get; set; }

        public PracticalCourse? PracticalCourse { get; set; }
    }
}