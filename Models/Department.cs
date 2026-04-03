namespace FirstAidAPI.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;  // "Khoa Nội", "Khoa Ngoại"
        public string? Description { get; set; }
        public string? Location { get; set; }  // "Tầng 2, Khu A"
        public int? HeadDoctorId { get; set; }  // FK to Doctor (nullable)
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Doctor? HeadDoctor { get; set; }

        public List<Doctor> Doctors { get; set; } = new();
        public List<Nurse> Nurses { get; set; } = new();
        public List<Speciality> Specialties { get; set; } = new();
        public List<Clinic> Clinics { get; set; } = new();   // phòng khám ngoại trú
        public List<Ward> Wards { get; set; } = new(); // phòng bệnh nội trú
    }
}
