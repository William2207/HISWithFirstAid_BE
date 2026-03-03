namespace FirstAidAPI.Models
{
    public class VitalSign
    {
        public int Id { get; set; }
        public int MedicalRecordId { get; set; }
        public int? NurseId { get; set; }

        public decimal? Temperature { get; set; }  // °C
        public string? BloodPressure { get; set; }  // "120/80"
        public int? HeartRate { get; set; }  // bpm
        public int? RespiratoryRate { get; set; }
        public decimal? Weight { get; set; }  // kg
        public decimal? Height { get; set; }  // cm
        public decimal? SpO2 { get; set; }  // %

        public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public MedicalRecord MedicalRecord { get; set; } = null!;

        public Nurse? Nurse { get; set; }
    }
}
