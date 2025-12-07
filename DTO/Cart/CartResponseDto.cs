namespace FirstAidAPI.DTO.Cart
{
    public class CartResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateOnly CreatedAt { get; set; }
        public DateOnly UpdatedAt { get; set; }
        public List<CartItemResponseDto> CartItems { get; set; } = new();
        public decimal TotalAmount { get; set; } // Tổng tiền
        public int TotalItems { get; set; } // Tổng số items
    }
}