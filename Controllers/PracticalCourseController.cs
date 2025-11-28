using Microsoft.AspNetCore.Mvc;
using FirstAidAPI.Service;
using FirstAidAPI.DTO.PracticalCourse;

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
    }
}