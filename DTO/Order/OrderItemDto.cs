namespace FirstAidAPI.DTO.Order
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int PracticalCourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; } // Giá tại thời điểm mua
        public decimal Subtotal { get; set; } // = Price * Quantity
    }
}