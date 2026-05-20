namespace FirstAidAPI.Models
{
    /// <summary>
    /// Ghi chú lâm sàng (Clinical Note): bác sĩ/điều dưỡng ghi chú theo giường bệnh,
    /// các ca khác có thể đọc lại để theo dõi tình trạng bệnh nhân liên tục.
    /// </summary>
    public class WardNote
    {
        public int Id { get; set; }

        public int AdmissionRecordId { get; set; }  // FK → AdmissionRecord
        public int AuthorUserId { get; set; }         // FK → User (Doctor hoặc Nurse)

        /// <summary>"DOCTOR" | "NURSE"</summary>
        public string AuthorRole { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public AdmissionRecord AdmissionRecord { get; set; } = null!;
        public User Author { get; set; } = null!;
    }
}
