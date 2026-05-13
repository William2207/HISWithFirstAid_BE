namespace FirstAidAPI.DTO.Admission
{
    /// <summary>
    /// Thông tin bệnh nhân đang chờ được đăng ký giường (bác sĩ đã chỉ định nhập viện).
    /// </summary>
    public class PendingAdmissionDto
    {
        public int MedicalRecordId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string? DiagnosisName { get; set; }
        public string? DoctorName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
