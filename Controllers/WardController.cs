using FirstAidAPI.DTO.Ward;
using FirstAidAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WardController : ControllerBase
    {
        private readonly IWardService _wardService;
        private readonly IDoctorService _doctorService;

        public WardController(IWardService wardService, IDoctorService doctorService)
        {
            _wardService = wardService;
            _doctorService = doctorService;
        }

        private int GetCurrentUserId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(claim) || !int.TryParse(claim, out var id))
                throw new UnauthorizedAccessException("Invalid token.");
            return id;
        }

        private string GetCurrentUserRole()
        {
            return User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
        }

        // ─── Admin Wards ──────────────────────────────────────────────────────────

        [HttpGet("admin/all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllWardsForAdmin()
        {
            var wards = await _wardService.GetAllWardsForAdminAsync();
            return Ok(wards);
        }

        [HttpPost("admin/create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateWardAdmin([FromBody] CreateWardAdminRequest request)
        {
            var result = await _wardService.CreateWardAdminAsync(request);
            return Ok(result);
        }

        [HttpPut("admin/update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateWardAdmin(int id, [FromBody] UpdateWardAdminRequest request)
        {
            try
            {
                var result = await _wardService.UpdateWardAdminAsync(id, request);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("admin/delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteWardAdmin(int id)
        {
            try
            {
                var success = await _wardService.DeleteWardAdminAsync(id);
                if (!success) return NotFound(new { message = "Ward not found" });
                return Ok(new { message = "Ward deleted successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ─── Rooms ────────────────────────────────────────────────────────────────

        /// <summary>
        /// Lấy danh sách tóm tắt tất cả phòng bệnh (tên phòng, loại, số bệnh nhân).
        /// Dùng cho màn hình chọn phòng (Ward Selection).
        /// </summary>
        [HttpGet("rooms")]
        public async Task<IActionResult> GetRooms()
        {
            var userId = GetCurrentUserId();
            var role = GetCurrentUserRole();
            var rooms = await _wardService.GetRoomSummariesAsync(userId, role);
            return Ok(rooms);
        }

        /// <summary>
        /// Lấy danh sách bệnh nhân đang nằm trong một phòng cụ thể, kèm sinh hiệu mới nhất.
        /// </summary>
        [HttpGet("rooms/{roomNumber}/patients")]
        public async Task<IActionResult> GetPatientsByRoom(string roomNumber)
        {
            var patients = await _wardService.GetPatientsByRoomAsync(roomNumber);
            return Ok(patients);
        }

        // ─── Orders ───────────────────────────────────────────────────────────────

        /// <summary>
        /// Lấy danh sách y lệnh của một bệnh nhân nội trú (theo AdmissionRecord).
        /// </summary>
        [HttpGet("admissions/{admissionRecordId}/orders")]
        public async Task<IActionResult> GetOrders(int admissionRecordId)
        {
            var orders = await _wardService.GetOrdersByAdmissionAsync(admissionRecordId);
            return Ok(orders);
        }

        /// <summary>
        /// Bác sĩ tạo y lệnh mới cho bệnh nhân nội trú.
        /// </summary>
        [HttpPost("orders")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateWardOrderRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                var order = await _wardService.CreateOrderAsync(userId, request);
                return CreatedAtAction(nameof(GetOrders),
                    new { admissionRecordId = order.AdmissionRecordId }, order);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Bác sĩ chỉnh sửa nội dung y lệnh (chỉ bác sĩ tạo lệnh mới được phép, và chỉ khi chưa hoàn thành/huỷ).
        /// </summary>
        [HttpPut("orders/{orderId}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] UpdateWardOrderRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                var order = await _wardService.UpdateOrderAsync(orderId, userId, request);
                return Ok(order);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Cập nhật trạng thái y lệnh (điều dưỡng thực hiện hoặc bác sĩ huỷ).
        /// </summary>
        [HttpPatch("orders/{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateWardOrderStatusRequest request)
        {
            try
            {
                var order = await _wardService.UpdateOrderStatusAsync(orderId, request.Status);
                return Ok(order);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // ─── Notes ────────────────────────────────────────────────────────────────

        /// <summary>
        /// Lấy danh sách ghi chú lâm sàng của một bệnh nhân nội trú.
        /// </summary>
        [HttpGet("admissions/{admissionRecordId}/notes")]
        public async Task<IActionResult> GetNotes(int admissionRecordId)
        {
            var notes = await _wardService.GetNotesByAdmissionAsync(admissionRecordId);
            return Ok(notes);
        }

        /// <summary>
        /// Bác sĩ hoặc điều dưỡng thêm ghi chú lâm sàng cho bệnh nhân nội trú.
        /// </summary>
        [HttpPost("notes")]
        public async Task<IActionResult> CreateNote([FromBody] CreateWardNoteRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                var role = GetCurrentUserRole();
                // Normalize role to DOCTOR or NURSE label
                var authorRole = role.Equals("Doctor", StringComparison.OrdinalIgnoreCase)
                    ? "DOCTOR" : "NURSE";

                var note = await _wardService.CreateNoteAsync(userId, authorRole, request);
                return CreatedAtAction(nameof(GetNotes),
                    new { admissionRecordId = note.AdmissionRecordId }, note);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // ─── Vital Signs ──────────────────────────────────────────────────────────

        /// <summary>
        /// Lấy lịch sử sinh hiệu của một bệnh nhân nội trú.
        /// </summary>
        [HttpGet("admissions/{admissionRecordId}/vitals")]
        public async Task<IActionResult> GetVitalsHistory(int admissionRecordId)
        {
            var vitals = await _wardService.GetVitalsHistoryByAdmissionAsync(admissionRecordId);
            return Ok(vitals);
        }

        /// <summary>
        /// Lưu sinh hiệu mới cho bệnh nhân nội trú.
        /// </summary>
        [HttpPost("admissions/{admissionRecordId}/vitals")]
        public async Task<IActionResult> LogVitals(int admissionRecordId, [FromBody] LogVitalsRequest request)
        {
            try
            {
                var role = GetCurrentUserRole();
                int? nurseUserId = null;

                if (role.Equals("Nurse", StringComparison.OrdinalIgnoreCase))
                {
                    nurseUserId = GetCurrentUserId();
                }

                var vitals = await _wardService.LogVitalsAsync(nurseUserId, admissionRecordId, request);
                return Ok(vitals);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
