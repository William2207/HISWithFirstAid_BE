namespace FirstAidAPI.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }  // ⭐ FK to PatientProfile
        public int DoctorId { get; set; }
        public int SpecialtyId { get; set; }
        public int? RoomId { get; set; }
        public int? QueueId { get; set; }

        public DateTime AppointmentDate { get; set; }
        public TimeSpan? AppointmentTime { get; set; }  // null = walk-in
        public string Type { get; set; } = "WALK_IN";  // "WALK_IN", "ONLINE"
        public string Status { get; set; } = "REGISTERED";  // "REGISTERED", "CONFIRMED", "CHECKED_IN", "IN_PROGRESS", "COMPLETED", "CANCELLED"

        public DateTime? CheckedInAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? Notes { get; set; }
        public string? CancellationReason { get; set; }

        public bool DocumentsHeld { get; set; } = false;
        public DateTime? DocumentsReturnedAt { get; set; }

        public int CreatedBy { get; set; }  // FK to User
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Patient Patient { get; set; } = null!;

        public Doctor Doctor { get; set; } = null!;
        public Specialty Specialty { get; set; } = null!;
        public Room? Room { get; set; }
        public Queue? Queue { get; set; }
        public User Creator { get; set; } = null!;

        public MedicalRecord? MedicalRecord { get; set; }  // 1:1
        public Invoice? Invoice { get; set; }  // 1:1
    }
}
