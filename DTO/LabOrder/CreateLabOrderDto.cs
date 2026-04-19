namespace FirstAidAPI.DTO.LabOrder
{
    public class CreateLabOrderDto
    {
        public int AppointmentId { get; set; }
        public List<LabOrderItemInputDto> Items { get; set; } = new();
    }

    public class LabOrderItemInputDto
    {
        public int MedicalServiceId { get; set; }
        public int Quantity { get; set; } = 1;
        public string? Note { get; set; }
    }
}
