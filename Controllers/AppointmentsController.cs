using FirstAidAPI.DTO.Appointment;
using FirstAidAPI.Exceptions;
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

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
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
                // In a real scenario you'd verify if this appointment belongs to the calling Doctor.
                var appointment = await _appointmentService.CompleteAppointmentAsync(id);
                return Ok(appointment);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
