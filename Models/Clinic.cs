namespace FirstAidAPI.Models
{
    public class Clinic
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = string.Empty;  // "P.101"
        public int DepartmentId { get; set; }   // FK
        public int SpecialtyId { get; set; }    // FK → gắn thẳng vào chuyên khoa
        public int Floor { get; set; }
        public string Status { get; set; } = "AVAILABLE";  // "AVAILABLE", "IN_USE", "CLOSED"

        // Navigation
        public Department Department { get; set; } = null!;
        public Specialty Specialty { get; set; } = null!;
        public List<Appointment> Appointments { get; set; } = new();
    }
}
