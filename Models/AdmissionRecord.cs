namespace FirstAidAPI.Models
{
    /// <summary>
    /// Lưu lịch sử nhập viện: ai (bệnh nhân), giường nào, do y tá nào xác nhận, và thời gian xuất viện.
    /// </summary>
    public class AdmissionRecord
    {
        public int Id { get; set; }

        public int PatientId { get; set; }         // FK → Patient
        public int BedId { get; set; }             // FK → Bed
        public int MedicalRecordId { get; set; }   // FK → MedicalRecord (chứa chỉ định nhập viện)
        public int AdmittedByNurseId { get; set; } // FK → Nurse (y tá thực hiện đăng ký)

        public DateTime AdmittedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DischargedAt { get; set; } // null = đang nằm viện

        public string? Notes { get; set; }

        // Navigation
        public Patient Patient { get; set; } = null!;
        public Bed Bed { get; set; } = null!;
        public MedicalRecord MedicalRecord { get; set; } = null!;
        public Nurse AdmittedByNurse { get; set; } = null!;
    }
}
