using FirstAidAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet("lookup")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDoctorsForLookup()
        {
            var doctors = await _doctorService.GetDoctorsForLookupAsync();
            return Ok(doctors);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableDoctors([FromQuery] int specialtyId, [FromQuery] DateTime date)
        {
            var result = await _doctorService.GetAvailableDoctorsAsync(specialtyId, date);
            return Ok(result);
        }

        [HttpGet("me")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            var profile = await _doctorService.GetDoctorProfileAsync(userId);
            if (profile == null)
            {
                return NotFound(new { message = "Doctor profile not found" });
            }

            return Ok(profile);
        }

        [HttpPut("me")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] FirstAidAPI.DTO.Doctor.UpdateDoctorProfileDto updateDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            var success = await _doctorService.UpdateDoctorProfileAsync(userId, updateDto);
            if (!success)
            {
                return BadRequest(new { message = "Failed to update profile" });
            }

            return Ok(new { message = "Profile updated successfully" });
        }
    }
}
