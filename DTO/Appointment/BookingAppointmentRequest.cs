namespace FirstAidAPI.DTO.Appointment
{
    public class BookingAppointmentRequest
    {
        public int DoctorId { get; set; }
        public int SpecialtyId { get; set; }
        public int? ClinicId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
    }
}
