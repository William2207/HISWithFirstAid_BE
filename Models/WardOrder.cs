namespace FirstAidAPI.Models
{
    /// <summary>
    /// Y lệnh nội trú (Medical Order): do bác sĩ tạo cho bệnh nhân đang nằm giường,
    /// điều dưỡng thực thi và cập nhật trạng thái.
    /// </summary>
    public class WardOrder
    {
        public int Id { get; set; }

        public int AdmissionRecordId { get; set; }  // FK → AdmissionRecord
        public int CreatedByDoctorId { get; set; }  // FK → Doctor

        public string OrderType { get; set; } = string.Empty;   // "MEDICATION" | "PROCEDURE" | "MONITORING" | "IV"
        public string Title { get; set; } = string.Empty;       // "Administer 5mg Lisinopril"
        public string Description { get; set; } = string.Empty; // "Oral, once daily - 09:00 AM"

        /// <summary>"PENDING" | "DUE" | "ACTIVE" | "COMPLETED" | "CANCELLED"</summary>
        public string Status { get; set; } = "PENDING";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        // Navigation
        public AdmissionRecord AdmissionRecord { get; set; } = null!;
        public Doctor CreatedByDoctor { get; set; } = null!;
    }
}
