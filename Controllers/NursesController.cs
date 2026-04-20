using FirstAidAPI.DTO.Nurse;
using FirstAidAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NursesController : ControllerBase
    {
        private readonly INurseService _nurseService;

        public NursesController(INurseService nurseService)
        {
            _nurseService = nurseService;
        }

        [HttpGet("me")]
        [Authorize(Roles = "Nurse")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            var profile = await _nurseService.GetNurseProfileAsync(userId);
            if (profile == null)
            {
                return NotFound(new { message = "Nurse profile not found" });
            }

            return Ok(profile);
        }

        [HttpPut("me")]
        [Authorize(Roles = "Nurse")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateNurseProfileDto updateDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            var success = await _nurseService.UpdateNurseProfileAsync(userId, updateDto);
            if (!success)
            {
                return BadRequest(new { message = "Failed to update profile" });
            }

            return Ok(new { message = "Profile updated successfully" });
        }
    }
}
