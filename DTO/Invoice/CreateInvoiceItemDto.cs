namespace FirstAidAPI.DTO.Invoice
{
    public class CreateInvoiceItemDto
    {
        public int? SpecilityId { get; set; }
        public int? MedicalServiceId { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
    }
}
