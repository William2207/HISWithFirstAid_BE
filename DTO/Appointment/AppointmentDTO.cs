using FirstAidAPI.Enums;

namespace FirstAidAPI.DTO.Appointment
{
    public class AppointmentDTO
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int SpecialtyId { get; set; }
        public int? ClinicId { get; set; }

        public DateTime AppointmentDateTime { get; set; }
        public AppointmentType Type { get; set; }
        public AppointmentStatus Status { get; set; }

        public string PatientName { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public string SpecialtyName { get; set; } = string.Empty;
    }
}
