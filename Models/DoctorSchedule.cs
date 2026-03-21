namespace FirstAidAPI.Models
{
    public class DoctorSchedule
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int? ClinicId { get; set; }
        public bool IsAvailable { get; set; } = true;

        // Navigation
        public Doctor Doctor { get; set; } = null!;

        public Clinic? Clinic { get; set; }
    }
}
