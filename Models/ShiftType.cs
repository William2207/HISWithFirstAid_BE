namespace FirstAidAPI.Models
{
    public class ShiftType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Sáng / Chiều / Đêm
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsNightShift { get; set; } = false;
        public bool IsActive { get; set; } = true;

        // Navigation
        public List<DoctorSchedule> DoctorSchedules { get; set; } = new();
    }
}
