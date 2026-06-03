namespace FirstAidAPI.DTO.Ward
{
    /// <summary>
    /// Thông tin bệnh nhân nội trú được hiển thị trên Ward Dashboard (một giường).
    /// </summary>
    public class WardPatientDto
    {
        public int AdmissionRecordId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string BedNumber { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;
        public string WardType { get; set; } = string.Empty;
        public int PatientAge { get; set; }
        public string PatientGender { get; set; } = string.Empty;
        public DateTime AdmittedAt { get; set; }
        public string? DiagnosisName { get; set; }
        public string? DoctorName { get; set; }

        // Sinh hiệu mới nhất
        public VitalsDto? LatestVitals { get; set; }
    }

    public class VitalsDto
    {
        public int Id { get; set; }
        public int? HeartRate { get; set; }
        public string? BloodPressure { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? SpO2 { get; set; }
        public int? RespiratoryRate { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public DateTime? RecordedAt { get; set; }
        public string? RecordedBy { get; set; }
    }
}
