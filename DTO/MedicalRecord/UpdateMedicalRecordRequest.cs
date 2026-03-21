namespace FirstAidAPI.DTO.MedicalRecord
{
    public class UpdateMedicalRecordRequest
    {
        // 1. Triệu chứng & Khám lâm sàng
        public string? ChiefComplaint { get; set; }
        public string? MedicalHistory { get; set; }
        public string? FamilyHistory { get; set; }
        public string? Symptoms { get; set; }
        public string? PhysicalExamination { get; set; }

        // 2. Chẩn đoán
        public string? DiagnosisName { get; set; }
        public string? DiagnosisNotes { get; set; }

        // 3. Đơn thuốc
        public string? Prescription { get; set; }

        // 4. Chỉ dẫn & Theo dõi
        public string? TreatmentPlan { get; set; }
        public string? FollowUpInstructions { get; set; }
        public DateTime? NextAppointmentDate { get; set; }

        // 5. Ghi chú chung
        public string? GeneralNotes { get; set; }

        public List<CreateVitalSignRequest>? VitalSigns { get; set; }
    }
}
