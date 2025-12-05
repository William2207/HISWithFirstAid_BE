using FirstAidAPI.DTO.Enrollment;
using FirstAidAPI.Extensions;
using FirstAidAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly ILogger<EnrollmentController> _logger;

        public EnrollmentController(IEnrollmentService enrollmentService, ILogger<EnrollmentController> logger)
        {
            _enrollmentService = enrollmentService;
            _logger = logger;
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetUserEnrollmentsAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //Console.WriteLine("UserId from token: " + userIdClaim);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid token" });
            }
            try
            {
                var enrollments = await _enrollmentService.GetUserEnrollmentsAsync(userId);
                return Ok(new { success = true, data = enrollments });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching enrollments for user {UserId}", userId);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("review/add")]
        [Authorize]
        public async Task<IActionResult> AddReviewAsync([FromBody] AddReviewRequest request)
        {
            try
            {
                await _enrollmentService.AddReviewAsync(
                    request.EnrollmentId,
                    request.Rating,
                    request.Review
                );
                return Ok(new { success = true, message = "Review added successfully" });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding review for enrollment {EnrollmentId}", request.EnrollmentId);
                return StatusCode(500, new { success = false, message = "An error occurred" });
            }
        }
    }
}