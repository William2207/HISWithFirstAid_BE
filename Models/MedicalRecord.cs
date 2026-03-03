namespace FirstAidAPI.Models
{
    public class MedicalRecord
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }  // FK
        public int DoctorId { get; set; }

        // ═══════════════════════════════════════
        // 1. TRIỆU CHỨNG & KHÁM LÂM SÀNG
        // ═══════════════════════════════════════
        public string? ChiefComplaint { get; set; }  // Lý do khám

        public string? Symptoms { get; set; }  // Triệu chứng
        public string? PhysicalExamination { get; set; }  // Khám lâm sàng

        // ═══════════════════════════════════════
        // 2. CHẨN ĐOÁN (GỘP LUÔN)
        // ═══════════════════════════════════════
        public string? DiagnosisICD10 { get; set; }  // Mã bệnh ICD-10

        public string? DiagnosisName { get; set; }  // Tên chẩn đoán
        public string? DiagnosisNotes { get; set; }  // Ghi chú chẩn đoán

        // ═══════════════════════════════════════
        // 3. ĐƠN THUỐC (TEXT/JSON - GỘP LUÔN)
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
        // 5. GHI CHÚ CHUNG
        // ═══════════════════════════════════════
        public string? GeneralNotes { get; set; }

        // ═══════════════════════════════════════
        // 6. METADATA
        // ═══════════════════════════════════════
        public string Status { get; set; } = "DRAFT";  // "DRAFT", "FINALIZED"

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? FinalizedAt { get; set; }

        // Navigation
        public Appointment Appointment { get; set; } = null!;

        public Doctor Doctor { get; set; } = null!;

        public List<VitalSign> VitalSigns { get; set; } = new();
        //public List<LabOrder> LabOrders { get; set; } = new();
    }
}
