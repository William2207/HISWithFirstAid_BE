namespace FirstAidAPI.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public int UserId { get; set; }  // FK → Users
        public int SpecialtyId { get; set; }  // FK
        public string LicenseNumber { get; set; } = string.Empty;  // Số chứng chỉ hành nghề
        public string? Qualifications { get; set; }  // Bằng cấp
        public int YearsOfExperience { get; set; }

        public bool IsAvailable { get; set; } = true;

        // Navigation
        public User User { get; set; } = null!;

        public Speciality? HeadOfSpeciality { get; set; }
        public Speciality Specialty { get; set; } = null!;
        public List<Appointment> Appointments { get; set; } = new();
        public List<MedicalRecord> MedicalRecords { get; set; } = new();
        public List<DoctorSchedule> Schedules { get; set; } = new();
    }
}
