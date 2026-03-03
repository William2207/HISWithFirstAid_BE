namespace FirstAidAPI.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = string.Empty;  // "P.301"
        public int DepartmentId { get; set; }  // FK
        public string RoomType { get; set; } = string.Empty;  // "EXAMINATION", "INPATIENT", "ICU", "EMERGENCY"
        public int Capacity { get; set; }  // Số giường
        public bool IsAvailable { get; set; } = true;

        // Navigation
        public Department Department { get; set; } = null!;

        public List<Bed> Beds { get; set; } = new();
        public List<Appointment> Appointments { get; set; } = new();
    }
}
