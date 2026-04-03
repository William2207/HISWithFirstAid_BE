using FirstAidAPI.Enums;

namespace FirstAidAPI.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int PatientId { get; set; }  // ⭐ FK to PatientProfile

        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus Status { get; set; }

        public string? TransactionId { get; set; }
        public DateTime PaidAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Invoice Invoice { get; set; } = null!;

        public Patient Patient { get; set; } = null!;
    }
}
