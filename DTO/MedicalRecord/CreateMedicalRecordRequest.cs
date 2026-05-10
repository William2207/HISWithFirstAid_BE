namespace FirstAidAPI.DTO.MedicalRecord
{
    public class CreateMedicalRecordRequest
    {
        public int AppointmentId { get; set; }

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

        // 5. GHI CHÚ CHUNG & QUYẾT ĐỊNH
        public string? GeneralNotes { get; set; }
        public bool IsHospitalized { get; set; } = false;

        // Dấu hiệu sinh tồn (tùy chọn)
        public CreateVitalSignRequest? VitalSigns { get; set; }
    }

    public class CreateVitalSignRequest
    {
        public int? NurseId { get; set; }
        public string? BloodPressure { get; set; }    // VD: "120/80"
        public int? HeartRate { get; set; }           // bpm
        public decimal? Temperature { get; set; }     // °C
        public int? RespiratoryRate { get; set; }     // Breaths/min
        public decimal? SpO2 { get; set; }            // %
        public decimal? Weight { get; set; }          // kg
        public decimal? Height { get; set; }          // cm
    }
}
