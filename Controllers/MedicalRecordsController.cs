using FirstAidAPI.DTO.MedicalRecord;
using FirstAidAPI.Exceptions;
using FirstAidAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Yêu cầu đăng nhập mặc định
    public class MedicalRecordsController : ControllerBase
    {
        private readonly IMedicalRecordService _medicalRecordService;

        public MedicalRecordsController(IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }

        private int GetCurrentUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                throw new UnauthorizedException("Không tìm thấy thông tin xác thực.");
            }
            return userId;
        }

        /// <summary>
        /// Lấy chi tiết bệnh án theo ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalRecordDTO>> GetById(int id)
        {
            var record = await _medicalRecordService.GetMedicalRecordByIdAsync(id);
            return Ok(record);
        }

        /// <summary>
        /// Lấy chi tiết bệnh án theo Appointment ID
        /// </summary>
        [HttpGet("appointment/{appointmentId}")]
        public async Task<ActionResult<MedicalRecordDTO>> GetByAppointmentId(int appointmentId)
        {
            var record = await _medicalRecordService.GetMedicalRecordByAppointmentIdAsync(appointmentId);
            return Ok(record);
        }

        /// <summary>
        /// Tạo mới bệnh án cho một lịch hẹn (Chỉ Doctor)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<MedicalRecordDTO>> Create([FromBody] CreateMedicalRecordRequest request)
        {
            try
            {
                var doctorId = GetCurrentUserId();
                var record = await _medicalRecordService.CreateMedicalRecordAsync(doctorId, request);
                return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Cập nhật bệnh án (Chỉ Doctor đã tạo mới được cập nhật)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<MedicalRecordDTO>> Update(int id, [FromBody] UpdateMedicalRecordRequest request)
        {
            try
            {
                var doctorId = GetCurrentUserId();
                var record = await _medicalRecordService.UpdateMedicalRecordAsync(id, doctorId, request);
                return Ok(record);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
