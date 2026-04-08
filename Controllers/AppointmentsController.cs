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
        private readonly IPatientRepository _patientRepository;

        public AppointmentsController(IAppointmentService appointmentService, IPatientRepository patientRepository)
        {
            _appointmentService = appointmentService;
            _patientRepository = patientRepository;
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
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetWaitingAppointments()
        {
            var doctorId = GetCurrentUserId();
            var appointments = await _appointmentService.GetWaitingAppointmentsByDoctorAsync(doctorId);
            return Ok(appointments);
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
                var patient = await _patientRepository.GetByUserIdAsync(userId);
                if (patient == null)
                    return NotFound(new { message = "Không tìm thấy hồ sơ bệnh nhân." });

                var appointments = await _appointmentService.GetAppointmentsByPatientAsync(patient.Id);
                return Ok(appointments);
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
