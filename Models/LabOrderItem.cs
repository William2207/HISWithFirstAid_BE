namespace FirstAidAPI.Models
{
    public class LabOrderItem
    {
        public int Id { get; set; }
        public int LabOrderId { get; set; }
        public int MedicalServiceId { get; set; }
        public string? Note { get; set; }

        public decimal UnitPrice { get; set; }    // Snapshot giá lúc tạo
        public int Quantity { get; set; } = 1;
        public decimal Amount { get; set; }       // UnitPrice * Quantity

        // Navigation
        public LabOrder LabOrder { get; set; } = null!;

        public MedicalService MedicalService { get; set; } = null!;
    }
}
