using FirstAidAPI.Enums;

namespace FirstAidAPI.Models
{
    public class LabOrder
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }

        public LabOrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Appointment Appointment { get; set; } = null!;

        public List<LabOrderItem> Items { get; set; } = new();
    }
}
