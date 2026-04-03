namespace FirstAidAPI.Models
{
    public class Speciality
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;  // "Tim mạch", "Tiêu hóa"
        public string? Description { get; set; }
        public int? DepartmentId { get; set; }  // FK (optional)
        public bool IsActive { get; set; } = true;
        public decimal Price { get; set; }

        // Navigation
        public Department? Department { get; set; }

        public List<DoctorSpecialty> DoctorSpecialties { get; set; } = new();
        public List<Appointment> Appointments { get; set; } = new();
    }
}
