using System.ComponentModel.DataAnnotations.Schema;

namespace FirstAidAPI.Models
{
    public class CartItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CartId { get; set; }
        public int PracticalCourseId { get; set; }

        public int Quantity { get; set; } = 1; // Với khóa học thường là 1
        public decimal Price { get; set; } // Lưu giá tại thời điểm thêm vào giỏ

        public DateOnly AddedAt { get; set; }

        // Navigation properties
        public Cart? Cart { get; set; }

        public PracticalCourse? PracticalCourse { get; set; }
    }
}