using FirstAidAPI.Enums;

namespace FirstAidAPI.DTO.Invoice
{
    public class PatientInvoiceDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string? PaymentUrl { get; set; }
    }
}
