using FirstAidAPI.DTO.LabOrder;
using FirstAidAPI.Exceptions;
using FirstAidAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabOrdersController : ControllerBase
    {
        private readonly ILabOrderService _labOrderService;
        private readonly ILogger<LabOrdersController> _logger;

        public LabOrdersController(ILabOrderService labOrderService, ILogger<LabOrdersController> logger)
        {
            _labOrderService = labOrderService;
            _logger = logger;
        }

        private int GetCurrentUserId()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
                throw new UnauthorizedException("Không tìm thấy thông tin xác thực.");
            return userId;
        }

        /// <summary>
        /// Bác sĩ tạo chỉ định xét nghiệm / dịch vụ cho bệnh nhân trong lịch hẹn
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateLabOrder([FromBody] CreateLabOrderDto dto)
        {
            try
            {
                var doctorUserId = GetCurrentUserId();
                var result = await _labOrderService.CreateLabOrderAsync(dto, doctorUserId);
                return CreatedAtAction(nameof(GetByAppointmentId),
                    new { appointmentId = result.AppointmentId },
                    new { success = true, data = result });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { success = false, message = ex.Message });
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo chỉ định LabOrder");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi tạo chỉ định." });
            }
        }

        /// <summary>
        /// Lấy danh sách chỉ định theo lịch hẹn
        /// </summary>
        [HttpGet("appointment/{appointmentId:int}")]
        [Authorize]
        public async Task<IActionResult> GetByAppointmentId(int appointmentId)
        {
            try
            {
                var result = await _labOrderService.GetByAppointmentIdAsync(appointmentId);
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy chỉ định theo appointment {AppointmentId}", appointmentId);
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi." });
            }
        }

        /// <summary>
        /// Receptionist lấy danh sách chỉ định Pending chưa có hóa đơn
        /// </summary>
        [HttpGet("pending")]
        [Authorize]
        public async Task<IActionResult> GetPendingLabOrders()
        {
            try
            {
                var result = await _labOrderService.GetPendingLabOrdersAsync();
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách chỉ định pending");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi." });
            }
        }

        /// <summary>
        /// Lấy danh sách dịch vụ y tế đang hoạt động (cho bác sĩ chọn khi tạo chỉ định)
        /// </summary>
        [HttpGet("medical-services")]
        [Authorize]
        public async Task<IActionResult> GetMedicalServices()
        {
            try
            {
                var result = await _labOrderService.GetAllMedicalServicesAsync();
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách dịch vụ y tế");
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi." });
            }
        }
    }
}
