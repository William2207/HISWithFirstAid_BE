namespace FirstAidAPI.DTO.Ward
{
    // ─── Ward Order DTOs ────────────────────────────────────────────────────────

    public class WardOrderDto
    {
        public int Id { get; set; }
        public int AdmissionRecordId { get; set; }
        public int CreatedByDoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string OrderType { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }

    public class CreateWardOrderRequest
    {
        public int AdmissionRecordId { get; set; }
        /// <summary>"MEDICATION" | "PROCEDURE" | "MONITORING" | "IV"</summary>
        public string OrderType { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? ScheduledAt { get; set; }
    }

    public class UpdateWardOrderStatusRequest
    {
        /// <summary>"PENDING" | "DUE" | "ACTIVE" | "COMPLETED" | "CANCELLED"</summary>
        public string Status { get; set; } = string.Empty;
    }

    public class UpdateWardOrderRequest
    {
        public string OrderType { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? ScheduledAt { get; set; }
    }

    // ─── Ward Note DTOs ─────────────────────────────────────────────────────────

    public class WardNoteDto
    {
        public int Id { get; set; }
        public int AdmissionRecordId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string AuthorRole { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class CreateWardNoteRequest
    {
        public int AdmissionRecordId { get; set; }
        public string Content { get; set; } = string.Empty;
    }

    // ─── Ward Overview DTOs ──────────────────────────────────────────────────────

    public class WardRoomSummaryDto
    {
        public string RoomNumber { get; set; } = string.Empty;
        public string WardType { get; set; } = string.Empty;
        public int Floor { get; set; }
        public int PatientCount { get; set; }
        public int TotalBeds { get; set; }
    }

    public class LogVitalsRequest
    {
        public int? HeartRate { get; set; }
        public string? BloodPressure { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? SpO2 { get; set; }
        public int? RespiratoryRate { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
    }
}
