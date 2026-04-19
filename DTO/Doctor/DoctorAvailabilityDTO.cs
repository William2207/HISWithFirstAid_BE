namespace FirstAidAPI.DTO.Doctor
{
    public class DoctorAvailabilityDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SpecialtyName { get; set; } = string.Empty;
        public int? ClinicId { get; set; }
        public string ClinicRoom { get; set; } = string.Empty;
        public int? ClinicFloor { get; set; }
        public List<string> AvailableTimeSlots { get; set; } = new();
    }
}
