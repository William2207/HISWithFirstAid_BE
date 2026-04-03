using FirstAidAPI.Enums;

namespace FirstAidAPI.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public int? AppointmentId { get; set; }

        public int PatientId { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; } = 0;
        public decimal Total { get; set; }
        public decimal PaidAmount { get; set; } = 0;
        public decimal RemainingAmount { get; set; }

        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; set; }

        // Navigation
        public Appointment? Appointment { get; set; }

        public Patient Patient { get; set; } = null!;
        public List<InvoiceItem> Items { get; set; } = new();
        public List<Payment> Payments { get; set; } = new();
    }
}
