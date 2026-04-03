using FirstAidAPI.DTO.Order;
using FirstAidAPI.Enums;

namespace FirstAidAPI.DTO.Invoice
{
    public class CreateInvoiceResponseDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; } = 0;
        public decimal Total { get; set; }
        public OrderStatus Status { get; set; }
    }
}
