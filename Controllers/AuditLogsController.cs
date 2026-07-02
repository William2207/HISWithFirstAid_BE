using FirstAidAPI.Data;
using FirstAidAPI.DTO.AuditLog;
using FirstAidAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AuditLogsController : ControllerBase
    {
        private readonly FirstAidContext _context;

        public AuditLogsController(FirstAidContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuditLogs([FromQuery] string? tableName, [FromQuery] string? recordId)
        {
            var query = _context.AuditLogs.AsQueryable();

            if (!string.IsNullOrEmpty(tableName) && tableName != "ALL")
            {
                query = query.Where(a => a.TableName == tableName);
            }

            if (!string.IsNullOrEmpty(recordId))
            {
                query = query.Where(a => a.RecordId.Contains(recordId));
            }

            var logs = await query.OrderByDescending(a => a.ChangedAt).ToListAsync();

            // Lấy danh sách users để map tên
            var userIds = logs.Where(l => !string.IsNullOrEmpty(l.ChangedByUserId))
                              .Select(l => int.Parse(l.ChangedByUserId!))
                              .Distinct()
                              .ToList();
            
            var users = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id.ToString(), u => u.FullName);

            var dtos = logs.Select(l => new AuditLogDto
            {
                Id = l.Id,
                TableName = l.TableName,
                Action = l.Action,
                RecordId = l.RecordId,
                ChangedByUserId = l.ChangedByUserId,
                ChangedByUserName = !string.IsNullOrEmpty(l.ChangedByUserId) && users.ContainsKey(l.ChangedByUserId) 
                                    ? users[l.ChangedByUserId] : "Hệ thống",
                ChangedAt = l.ChangedAt,
                OldValues = l.OldValues,
                NewValues = l.NewValues
            }).ToList();

            return Ok(dtos);
        }
    }
}
