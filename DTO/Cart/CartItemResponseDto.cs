namespace FirstAidAPI.DTO.Cart
{
    public class CartItemResponseDto
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int PracticalCourseId { get; set; }
        public string CourseName { get; set; } = string.Empty; // Tên khóa học
        public string? CourseImage { get; set; } // Ảnh khóa học
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal { get; set; } // Price * Quantity
        public DateOnly AddedAt { get; set; }
    }
}