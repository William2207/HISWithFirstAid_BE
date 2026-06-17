using System;

namespace FirstAidAPI.Models
{
    public class AuditLog
    {
        public Guid Id { get; set; }
        public string TableName { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string RecordId { get; set; } = string.Empty;
        public string? ChangedByUserId { get; set; }
        public DateTime ChangedAt { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
    }
}
