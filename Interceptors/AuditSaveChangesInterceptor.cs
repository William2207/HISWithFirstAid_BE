using FirstAidAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Security.Claims;
using System.Text.Json;

namespace FirstAidAPI.Interceptors
{
    public class AuditSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditSaveChangesInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var dbContext = eventData.Context;
            if (dbContext == null) return base.SavingChangesAsync(eventData, result, cancellationToken);

            var auditLogs = new List<AuditLog>();
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            // Chỉnh sửa theo yêu cầu: Chỉ audit 3 bảng
            var auditedTables = new[] { "MedicalRecords", "WardOrders", "AdmissionRecords" };

            var entries = dbContext.ChangeTracker.Entries()
                .Where(e => e.Entity is not AuditLog && 
                           (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted))
                .ToList();

            foreach (var entry in entries)
            {
                var tableName = entry.Metadata.GetTableName();
                if (string.IsNullOrEmpty(tableName) || !auditedTables.Contains(tableName))
                {
                    continue;
                }

                var auditLog = new AuditLog
                {
                    Id = Guid.NewGuid(),
                    TableName = tableName,
                    Action = entry.State.ToString(),
                    ChangedAt = DateTime.UtcNow,
                    ChangedByUserId = userId
                };

                // Lấy RecordId (Khóa chính)
                var primaryKey = entry.Metadata.FindPrimaryKey();
                if (primaryKey != null)
                {
                    var keys = primaryKey.Properties.Select(p => entry.Property(p.Name).CurrentValue?.ToString());
                    auditLog.RecordId = string.Join(",", keys);
                }

                if (entry.State == EntityState.Added)
                {
                    auditLog.NewValues = JsonSerializer.Serialize(entry.CurrentValues.ToObject());
                }
                else if (entry.State == EntityState.Deleted)
                {
                    auditLog.OldValues = JsonSerializer.Serialize(entry.OriginalValues.ToObject());
                }
                else if (entry.State == EntityState.Modified)
                {
                    var oldValues = new Dictionary<string, object?>();
                    var newValues = new Dictionary<string, object?>();

                    foreach (var property in entry.Properties)
                    {
                        if (property.IsModified)
                        {
                            oldValues[property.Metadata.Name] = property.OriginalValue;
                            newValues[property.Metadata.Name] = property.CurrentValue;
                        }
                    }

                    if (oldValues.Count == 0) continue; // Không có gì thực sự đổi

                    auditLog.OldValues = JsonSerializer.Serialize(oldValues);
                    auditLog.NewValues = JsonSerializer.Serialize(newValues);
                }

                auditLogs.Add(auditLog);
            }

            if (auditLogs.Any())
            {
                dbContext.AddRange(auditLogs);
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
