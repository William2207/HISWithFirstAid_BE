namespace FirstAidAPI.DTO.Order
{
    public class CreateOrderItemDto
    {
        public int PracticalCourseId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}