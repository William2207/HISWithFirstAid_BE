namespace FirstAidAPI.DTO.Invoice
{
    public class InvoiceItemDto
    {
        public int Id { get; set; }
        public int SpecilityId { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal { get; set; } // = Price * Quantity
    }
}
