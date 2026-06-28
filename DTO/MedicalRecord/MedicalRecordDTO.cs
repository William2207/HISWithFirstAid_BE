namespace FirstAidAPI.DTO.MedicalRecord
{
    public class MedicalRecordDTO
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int DoctorId { get; set; }
        public string? DoctorName { get; set; }
        public string? DoctorSpecialty { get; set; }

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
        public bool IsHospitalized { get; set; }

        // Metadata
        public DateTime CreatedAt { get; set; }

        // Dấu hiệu sinh tồn
        public VitalSignDTO? VitalSigns { get; set; } = new();
    }

    public class VitalSignDTO
    {
        public int Id { get; set; }
        public string? BloodPressure { get; set; }
        public int? HeartRate { get; set; }
        public decimal? Temperature { get; set; }
        public int? RespiratoryRate { get; set; }
        public decimal? SpO2 { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public DateTime RecordedAt { get; set; }
    }
}

