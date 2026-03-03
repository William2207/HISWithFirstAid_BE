using FirstAidAPI.DTO;
using FirstAidAPI.DTO.PracticalCourse;
using FirstAidAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PracticalCourseController : Controller
    {
        private readonly IPracticalCourseService _practicalCourseService;
        private readonly ILogger<PracticalCourseController> _logger;

        public PracticalCourseController(IPracticalCourseService practicalCourseService, ILogger<PracticalCourseController> logger)
        {
            _practicalCourseService = practicalCourseService;
            _logger = logger;
        }

        // GET: api/practicalcourse/all
        [HttpGet("all")]
        [ProducesResponseType(typeof(IEnumerable<PracticalCourseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PracticalCourseDto>>> GetAllPracticeCourse()
        {
            try
            {
                var practicalCourses = await _practicalCourseService.GetAllPracticalCoursesAsync();
                return Ok(practicalCourses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all practical courses.");
                return StatusCode(500, "Internal server error");
            }
        }

        //GET: api/practicalcourse
        [HttpGet]
        public async Task<ActionResult<PagedResult<PracticalCourseDto>>> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
        {
            try
            {
                var result = await _practicalCourseService.GetPagedCoursesAsync(page, pageSize, search);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving paged courses", error = ex.Message });
            }
        }

        // GET: api/practicalcourse/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PracticalCourseDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<PracticalCourseDto>> GetPracticalCourseById(int id)
        {
            var practicalCourse = await _practicalCourseService.GetPracticalCourseByIdAsync(id);
            if (practicalCourse == null)
            {
                return NotFound();
            }
            return Ok(practicalCourse);
        }

        // POST: api/practicalcourse
        [HttpPost]
        public async Task<ActionResult<PracticalCourseDto>> Create([FromBody] CreatePracticalCourseDto dto)
        {
            try
            {
                var course = await _practicalCourseService.CreateCourseAsync(dto);
                return CreatedAtAction(nameof(GetPracticalCourseById), new { id = course.Id }, course);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating course", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PracticalCourseDto>> Update(int id, [FromBody] UpdatePracticalCourseDto dto)
        {
            try
            {
                var course = await _practicalCourseService.UpdateCourseAsync(id, dto);
                if (course == null)
                    return NotFound(new { message = $"Course with ID {id} not found" });

                return Ok(course);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating course", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await _practicalCourseService.DeleteCourseAsync(id);
                if (!result)
                    return NotFound(new { message = $"Course with ID {id} not found" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting course", error = ex.Message });
            }
        }

        [HttpPatch("{id}/publish")]
        public async Task<ActionResult> Publish(int id)
        {
            try
            {
                var result = await _practicalCourseService.PublishCourseAsync(id);
                if (!result)
                    return NotFound(new { message = $"Course with ID {id} not found" });

                return Ok(new { message = "Course published successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error publishing course", error = ex.Message });
            }
        }

        [HttpPatch("{id}/unpublish")]
        public async Task<ActionResult> Unpublish(int id)
        {
            try
            {
                var result = await _practicalCourseService.UnpublishCourseAsync(id);
                if (!result)
                    return NotFound(new { message = $"Course with ID {id} not found" });

                return Ok(new { message = "Course unpublished successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error unpublishing course", error = ex.Message });
            }
        }
    }
}
