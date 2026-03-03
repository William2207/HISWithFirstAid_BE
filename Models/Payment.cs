namespace FirstAidAPI.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int PatientId { get; set; }  // ⭐ FK to PatientProfile
        public int? CashierId { get; set; }  // FK to Receptionist

        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = "CASH";  // "CASH", "CARD", "BANK_TRANSFER", "VNPAY"
        public string Status { get; set; } = "PAID";

        public string? TransactionId { get; set; }
        public DateTime PaidAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Invoice Invoice { get; set; } = null!;

        public Patient Patient { get; set; } = null!;
        public Receptionist? Cashier { get; set; }
    }
}
