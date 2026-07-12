using FirstAidAPI.Enums;

namespace FirstAidAPI.DTO.LabOrder
{
    public class LabOrderResponseDto
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public LabOrderStatus Status { get; set; }
        public string StatusLabel { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<LabOrderItemResponseDto> Items { get; set; } = new();
    }

    public class LabOrderItemResponseDto
    {
        public int Id { get; set; }
        public int MedicalServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public string? Note { get; set; }
        public string? ResultImageUrl { get; set; }
        public string? ResultNote { get; set; }
        public string? ResultData { get; set; }
    }
}
