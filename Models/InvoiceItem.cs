namespace FirstAidAPI.Models
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }

        public string ItemType { get; set; } = string.Empty;  // "EXAMINATION", "LAB_TEST", "IMAGING"
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }

        public int? MedicalServiceId { get; set; }  // FK (nullable)

        // Navigation
        public Invoice Invoice { get; set; } = null!;
    }
}
