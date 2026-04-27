namespace FirstAidAPI.DTO.Doctor
{
    public class DoctorScheduleDto
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public string? ShiftName { get; set; }
        public string? ClinicRoom { get; set; }
        public int? SpecialtyId { get; set; }
        public bool IsOff { get; set; }
    }
}
