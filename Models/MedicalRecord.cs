namespace FirstAidAPI.Models
{
    public class MedicalRecord
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }  // FK
        public int DoctorId { get; set; }
        public int PatientId { get; set; }

        // ═══════════════════════════════════════
        // 1. TRIỆU CHỨNG & KHÁM LÂM SÀNG
        // ═══════════════════════════════════════
        public string? ChiefComplaint { get; set; }  // Lý do khám

        public string? MedicalHistory { get; set; }        // Tiền sử bệnh bản thân
        public string? FamilyHistory { get; set; }// Tiền sử gia đình
        public string? Symptoms { get; set; }  // Triệu chứng
        public string? PhysicalExamination { get; set; }  // Khám lâm sàng

        // ═══════════════════════════════════════
        // 2. CHẨN ĐOÁN
        // ═══════════════════════════════════════
        public string? DiagnosisName { get; set; }  // Tên chẩn đoán

        public string? DiagnosisNotes { get; set; }  // Ghi chú chẩn đoán

        // ═══════════════════════════════════════
        // 3. ĐƠN THUỐC (TEXT/JSON)
        // ═══════════════════════════════════════
        public string? Prescription { get; set; }  // Lưu dạng text hoặc JSON

        // VD JSON: [{"name":"Aspirin 100mg","dosage":"1 viên","frequency":"1 lần/ngày","duration":"30 ngày","instructions":"Uống sau ăn"}]

        // ═══════════════════════════════════════
        // 4. CHỈ DẪN & THEO DÕI
        // ═══════════════════════════════════════
        public string? TreatmentPlan { get; set; }  // Phác đồ điều trị

        public string? FollowUpInstructions { get; set; }  // Hướng dẫn tái khám
        public DateTime? NextAppointmentDate { get; set; }  // Ngày tái khám

        // ═══════════════════════════════════════
        // 5. GHI CHÚ CHUNG & QUYẾT ĐỊNH
        // ═══════════════════════════════════════
        public string? GeneralNotes { get; set; }

        public bool IsHospitalized { get; set; } = false; // Quyết định nhập viện

        // ═══════════════════════════════════════
        // 6. METADATA
        // ═══════════════════════════════════════

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Appointment Appointment { get; set; } = null!;

        public Patient Patient { get; set; } = null!;

        public Doctor Doctor { get; set; } = null!;

        public VitalSign VitalSigns { get; set; } = new();
    }
}
