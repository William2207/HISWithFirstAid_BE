using FirstAidAPI.Enums;

namespace FirstAidAPI.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }  // ⭐ FK to PatientProfile
        public int DoctorId { get; set; }
        public int SpecialtyId { get; set; }
        public int? ClinicId { get; set; }
        public int CreatorId { get; set; }

        public DateTime AppointmentDateTime { get; set; }
        public AppointmentType Type { get; set; }
        public AppointmentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public Patient Patient { get; set; } = null!;

        public Doctor Doctor { get; set; } = null!;
        public Speciality Specialty { get; set; } = null!;
        public Clinic? Clinic { get; set; }
        public User Creator { get; set; } = null!;

        public MedicalRecord? MedicalRecord { get; set; }  // 1:1
        public Invoice? Invoice { get; set; }  // 1:1
    }
}
