namespace FirstAidAPI.Models
{
    public class DoctorSpecialty
    {
        public int DoctorId { get; set; }
        public int SpecialtyId { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Doctor Doctor { get; set; } = null!;

        public Specialty Specialty { get; set; } = null!;
    }
}
