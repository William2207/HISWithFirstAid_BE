namespace FirstAidAPI.Models
{
    public class Nurse
    {
        public int Id { get; set; }
        public int UserId { get; set; }  // FK → Users
        public int DepartmentId { get; set; }  // FK
        public string LicenseNumber { get; set; } = string.Empty;
        public string? Qualifications { get; set; }
        public int YearsOfExperience { get; set; }
        public bool IsAvailable { get; set; } = true;

        // Navigation
        public Department Department { get; set; } = null!;

        public User User { get; set; } = null!;
        public List<VitalSign> VitalSignsRecorded { get; set; } = new();
        //public List<MedicationAdministration> MedicationAdministrations { get; set; } = new();
    }
}
