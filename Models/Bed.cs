namespace FirstAidAPI.Models
{
    public class Bed
    {
        public int Id { get; set; }
        public int RoomId { get; set; }  // FK
        public string BedNumber { get; set; } = string.Empty;  // "G01"
        public string Status { get; set; } = "AVAILABLE";  // "AVAILABLE", "OCCUPIED", "MAINTENANCE"
        public int? CurrentPatientId { get; set; }  // FK to Patient (nullable)

        // Navigation
        public Room Room { get; set; } = null!;

        public Patient? CurrentPatient { get; set; }
    }
}
