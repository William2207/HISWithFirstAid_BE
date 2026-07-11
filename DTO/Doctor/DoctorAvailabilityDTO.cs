namespace FirstAidAPI.DTO.Doctor
{
    public class TimeSlotDTO
    {
        public string Time { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
    }

    public class DoctorAvailabilityDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SpecialtyName { get; set; } = string.Empty;
        public int? ClinicId { get; set; }
        public string ClinicRoom { get; set; } = string.Empty;
        public int? ClinicFloor { get; set; }
        public List<TimeSlotDTO> TimeSlots { get; set; } = new();
    }
}
