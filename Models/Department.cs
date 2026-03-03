namespace FirstAidAPI.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;  // "Khoa Nội", "Khoa Ngoại"
        public string? Description { get; set; }
        public string? Location { get; set; }  // "Tầng 2, Khu A"
        public int? HeadDoctorId { get; set; }  // FK to Doctor (nullable)
        public int TotalBeds { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Doctor? HeadDoctor { get; set; }

        public List<Doctor> Doctors { get; set; } = new();
        public List<Nurse> Nurses { get; set; } = new();
        public List<Specialty> Specialties { get; set; } = new();
        public List<Room> Rooms { get; set; } = new();
    }
}
