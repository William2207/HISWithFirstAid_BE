using System.Security.Claims;
using FirstAidAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstAidAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ScheduleController : Controller
    {
        private readonly IScheduleService _scheduleService;
        private readonly IDoctorService _doctorService;

        public ScheduleController(IScheduleService scheduleService, IDoctorService doctorService)
        {
            _scheduleService = scheduleService;
            _doctorService = doctorService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromQuery] int month, [FromQuery] int year)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var profile = await _doctorService.GetDoctorProfileAsync(userId);
            if (profile == null) return NotFound("Doctor not found");

            if (!profile.IsHeadDoctor)
                return Forbid("Only Head Doctor can generate schedule.");

            await _scheduleService.GenerateSpecialtyScheduleAsync(profile.SpecialtyId, month, year);
            return Ok("Xếp lịch thành công!");
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMySchedule([FromQuery] int month, [FromQuery] int year)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var doctorId = await _doctorService.GetDoctorIdByUserId(userId);
            var result = await _scheduleService.GetDoctorScheduleAsync(doctorId, month, year);
            return Ok(result);
        }

        [HttpGet("specialty")]
        public async Task<IActionResult> GetSpecialtySchedule([FromQuery] int month, [FromQuery] int year)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var profile = await _doctorService.GetDoctorProfileAsync(userId);
            if (profile == null) return NotFound("Doctor not found");

            var result = await _scheduleService.GetSpecialtyScheduleAsync(profile.SpecialtyId, month, year);
            return Ok(result);
        }

        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthly([FromQuery] int month, [FromQuery] int year)
        {
            var result = await _scheduleService.GetMonthlyScheduleAsync(month, year);
            return Ok(result);
        }

        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetDoctorSchedule(
            int doctorId,
            [FromQuery] int month,
            [FromQuery] int year)
        {
            var result = await _scheduleService.GetDoctorScheduleAsync(doctorId, month, year);
            return Ok(result);
        }
    }
}
