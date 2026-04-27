using FirstAidAPI.DTO.Patient;
using FirstAidAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet("me")]
        [Authorize(Roles = "User, Patient")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            var profile = await _patientService.GetPatientProfileAsync(userId);
            if (profile == null)
            {
                return NotFound(new { message = "Patient profile not found" });
            }

            return Ok(profile);
        }

        [HttpPut("me")]
        [Authorize(Roles = "User, Patient")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdatePatientProfileDto updateDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            var success = await _patientService.UpdatePatientProfileAsync(userId, updateDto);
            if (!success)
            {
                return BadRequest(new { message = "Failed to update profile" });
            }

            return Ok(new { message = "Profile updated successfully" });
        }

        [HttpGet("specialty/{specialtyId}")]
        [Authorize(Roles = "Doctor, Admin")]
        public async Task<IActionResult> GetBySpecialty(int specialtyId)
        {
            var patients = await _patientService.GetPatientsBySpecialtyAsync(specialtyId);
            return Ok(patients);
        }
    }
}
