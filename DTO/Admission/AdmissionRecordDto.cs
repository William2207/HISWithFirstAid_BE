namespace FirstAidAPI.DTO.Admission
{
    /// <summary>
    /// Thông tin chi tiết một bản ghi nhập viện (dùng khi trả về sau khi gán giường).
    /// </summary>
    public class AdmissionRecordDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int BedId { get; set; }
        public string BedNumber { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;
        public string WardType { get; set; } = string.Empty;
        public int MedicalRecordId { get; set; }
        public string? DiagnosisName { get; set; }
        public string? DoctorName { get; set; }
        public int PatientAge { get; set; }
        public string PatientGender { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? IdCard { get; set; }
        public string? NurseName { get; set; }
        public DateTime AdmittedAt { get; set; }
        public DateTime? DischargedAt { get; set; }
        public string? Notes { get; set; }
    }
}
