namespace FirstAidAPI.DTO.Invoice
{
    public class CreateInvoiceDto
    {
        public int? AppointmentId { get; set; }
        public int PatientId { get; set; }
        public decimal Discount { get; set; } = 0;
        public FirstAidAPI.Enums.PaymentMethod? PaymentMethod { get; set; }
    }
}
