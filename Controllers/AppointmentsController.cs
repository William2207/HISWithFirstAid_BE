using FirstAidAPI.DTO.Appointment;
using FirstAidAPI.Exceptions;
using FirstAidAPI.Repository;
using FirstAidAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IDoctorService _doctorService;

        public AppointmentsController(IAppointmentService appointmentService, IDoctorService doctorService)
        {
            _appointmentService = appointmentService;
            _doctorService = doctorService;
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

        [HttpPost]
        public async Task<ActionResult<AppointmentDTO>> CreateAppointment([FromBody] CreateAppointmentRequest request)
        {
            try
            {
                var creatorId = GetCurrentUserId();
                var appointment = await _appointmentService.CreateAppointmentAsync(creatorId, request);
                return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, appointment);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDTO>> GetById(int id)
        {
            try
            {
                var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
                return Ok(appointment);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("doctor/waiting")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetWaitingAppointments([FromQuery] DateTime? date)
        {
            var userId = GetCurrentUserId();
            var doctorId = await _doctorService.GetDoctorIdByUserId(userId);

            IEnumerable<AppointmentDTO> appointments;
            if (date.HasValue)
            {
                appointments = await _appointmentService.GetAppointmentsByDoctorAndDateAsync(doctorId, date.Value);
            }
            else
            {
                appointments = await _appointmentService.GetWaitingAppointmentsByDoctorAsync(doctorId);
            }

            return Ok(appointments);
        }

        /// <summary>
        /// Doctor gọi bệnh nhân vào khám: chuyển Status → In_Progress, auto-tạo MedicalRecord trống.
        /// </summary>
        [HttpPut("{id}/start")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<AppointmentDTO>> StartAppointment(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var doctorId = await _doctorService.GetDoctorIdByUserId(userId);
                var appointment = await _appointmentService.StartAppointmentAsync(id, doctorId);
                return Ok(appointment);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/complete")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<AppointmentDTO>> CompleteAppointment(int id)
        {
            try
            {
                var appointment = await _appointmentService.CompleteAppointmentAsync(id);
                return Ok(appointment);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>Lấy danh sách lịch hẹn của bệnh nhân đang đăng nhập</summary>
        [HttpGet("patient/me")]
        public async Task<IActionResult> GetMyAppointments()
        {
            try
            {
                var userId = GetCurrentUserId();
                var appointments = await _appointmentService.GetAppointmentsByUserIdAsync(userId);
                return Ok(appointments);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        /// <summary>Lấy danh sách lịch hẹn đã hoàn thành (chưa tạo hóa đơn)</summary>
        [HttpGet("completed/no-invoice")]
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetCompletedAppointmentsNoInvoice()
        {
            var appointments = await _appointmentService.GetCompletedAppointmentsAsync();
            return Ok(appointments);
        }

        /// <summary>Hủy lịch hẹn</summary>
        [HttpDelete("{id}/cancel")]
        [Authorize(Roles = "Patient,Receptionist")]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            try
            {
                await _appointmentService.CancelAppointmentAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
