namespace FirstAidAPI.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public int UserId { get; set; }  // FK → Users
        public int DepartmentId { get; set; }  // FK
        public int PrimarySpecialtyId { get; set; }  // FK
        public string LicenseNumber { get; set; } = string.Empty;  // Số chứng chỉ hành nghề
        public string? Qualifications { get; set; }  // Bằng cấp
        public int YearsOfExperience { get; set; }

        public bool IsAvailable { get; set; } = true;

        // Navigation
        public Department Department { get; set; } = null!;

        public User User { get; set; } = null!;
        public Specialty PrimarySpecialty { get; set; } = null!;
        public List<DoctorSpecialty> DoctorSpecialties { get; set; } = new();  // Many-to-many
        public List<Appointment> Appointments { get; set; } = new();
        public List<MedicalRecord> MedicalRecords { get; set; } = new();
        public List<DoctorSchedule> Schedules { get; set; } = new();
    }
}
