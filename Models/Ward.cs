namespace FirstAidAPI.Models
{
    public class Ward
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = string.Empty;  // "P.301"
        public int DepartmentId { get; set; }   // FK
        public string WardType { get; set; } = string.Empty;  // "GENERAL", "ICU", "EMERGENCY"
        public int Floor { get; set; }

        // Navigation
        public Department Department { get; set; } = null!;

        public List<Bed> Beds { get; set; } = new();
    }
}
