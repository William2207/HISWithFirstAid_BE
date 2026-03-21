namespace FirstAidAPI.DTO.Appointment
{
    public class CreateAppointmentRequest
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int SpecialtyId { get; set; }
        public int? ClinicId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
    }
}
