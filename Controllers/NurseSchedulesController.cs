using System.Security.Claims;
using FirstAidAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstAidAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NurseSchedulesController : Controller
    {
        private readonly INurseScheduleService _scheduleService;
        private readonly INurseService _nurseService;

        public NurseSchedulesController(INurseScheduleService scheduleService, INurseService nurseService)
        {
            _scheduleService = scheduleService;
            _nurseService = nurseService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromQuery] int month, [FromQuery] int year)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var profile = await _nurseService.GetNurseProfileAsync(userId);
            if (profile == null) return NotFound("Nurse not found");

            if (!profile.IsHeadNurse)
                return Forbid("Only Head Nurse can generate schedule.");

            await _scheduleService.GenerateSpecialtyScheduleAsync(profile.SpecialtyId, month, year);
            return Ok("Xếp lịch thành công!");
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMySchedule([FromQuery] int month, [FromQuery] int year)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var nurseId = await _nurseService.GetNurseIdByUserId(userId);
            var result = await _scheduleService.GetNurseScheduleAsync(nurseId, month, year);
            return Ok(result);
        }

        [HttpGet("specialty")]
        public async Task<IActionResult> GetSpecialtySchedule([FromQuery] int month, [FromQuery] int year)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized();

            var profile = await _nurseService.GetNurseProfileAsync(userId);
            if (profile == null) return NotFound("Nurse not found");

            var result = await _scheduleService.GetSpecialtyScheduleAsync(profile.SpecialtyId, month, year);
            return Ok(result);
        }

        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthly([FromQuery] int month, [FromQuery] int year)
        {
            var result = await _scheduleService.GetMonthlyScheduleAsync(month, year);
            return Ok(result);
        }

        [HttpGet("nurse/{nurseId}")]
        public async Task<IActionResult> GetNurseSchedule(
            int nurseId,
            [FromQuery] int month,
            [FromQuery] int year)
        {
            var result = await _scheduleService.GetNurseScheduleAsync(nurseId, month, year);
            return Ok(result);
        }
    }
}
