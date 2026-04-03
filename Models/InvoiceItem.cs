namespace FirstAidAPI.Models
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int? SpecilityId { get; set; }  //phí khám chuyên khoa
        public int? MedicalServiceId { get; set; }  //phí dịch vụ (xét nghiệm, chẩn đoán hình ảnh, thủ thuật...)

        public string Description { get; set; } = string.Empty;  // Snapshot tên lúc tạo
        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }   // Snapshot giá lúc tạo
        public decimal Amount { get; set; }      // = UnitPrice * Quantity

        // Navigation
        public Invoice Invoice { get; set; } = null!;

        public Speciality? Specility { get; set; }
        public MedicalService? MedicalService { get; set; }
    }
}
