namespace FirstAidAPI.DTO.Appointment
{
    /// <summary>
    /// Receptionist tạo lịch hẹn walk-in.
    /// Nếu bệnh nhân đã có tài khoản thì truyền PatientId.
    /// Nếu là bệnh nhân vãng lai thì để PatientId = null và điền WalkIn fields.
    /// </summary>
    public class CreateAppointmentRequest
    {
        // ── Existing Patient (optional) ────────────────────────
        public int? PatientId { get; set; }

        // ── Walk-in Patient Info (required if PatientId == null) ──
        public string? FullName { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? InsuranceNumber { get; set; }
        public string? IdCard { get; set; }

        // ── Appointment Info ───────────────────────────────────
        public int DoctorId { get; set; }

        public int SpecialtyId { get; set; }
        public int? ClinicId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
    }
}
