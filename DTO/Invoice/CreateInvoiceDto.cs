namespace FirstAidAPI.DTO.Invoice
{
    public class CreateInvoiceDto
    {
        public int? AppointmentId { get; set; }
        public int PatientId { get; set; }
        public decimal Discount { get; set; } = 0;
        public List<CreateInvoiceItemDto> Items { get; set; } = new();
    }
}
