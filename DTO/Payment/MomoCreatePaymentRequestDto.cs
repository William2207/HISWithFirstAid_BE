namespace FirstAidAPI.DTO.Payment
{
    public class MomoCreatePaymentRequestDto
    {
        public string OrderNumber { get; set; } = string.Empty;
        public long Amount { get; set; }
        public string OrderDescription { get; set; } = string.Empty;
        public string? ReturnUrl { get; set; }
    }
}
