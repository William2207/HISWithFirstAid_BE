using FirstAidAPI.Data;
using FirstAidAPI.DTO;
using FirstAidAPI.DTO.Quiz;
using FirstAidAPI.Models;
using FirstAidAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : Controller
    {
        //private readonly FirstAidContext _context;
        private readonly IQuizQuestionService _service;

        private readonly ILogger<QuizController> _logger;

        public QuizController(
        IQuizQuestionService service,
        ILogger<QuizController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuizQuestionDto>> GetById(int id)
        {
            try
            {
                var question = await _service.GetByIdAsync(id);
                if (question == null)
                    return NotFound($"Quiz question with ID {id} not found");

                return Ok(question);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting quiz question {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("technique/{techniqueId}")]
        public async Task<ActionResult<IEnumerable<QuizQuestionDto>>> GetByTechniqueId(int techniqueId)
        {
            try
            {
                var questions = await _service.GetByTechniqueIdAsync(techniqueId);
                return Ok(questions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting quiz questions for technique {TechniqueId}", techniqueId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<QuizQuestionDto>>> GetAll()
        {
            try
            {
                var questions = await _service.GetAllAsync();
                return Ok(questions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all quiz questions");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<QuizQuestionDto>>> GetAllQuizQuestions(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _service.GetAllQuizQuestionsAsync(page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", detail = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<QuizQuestionDto>> Create([FromBody] CreateQuizQuestionDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var questionDto = await _service.CreateAsync(createDto);
                return CreatedAtAction(nameof(GetById), new { id = questionDto.Id }, questionDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating quiz question");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateQuizQuestionDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedQuestion = await _service.UpdateAsync(id, updateDto);
                return Ok(updatedQuestion);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating quiz question {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting quiz question {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("answeroption/{id}")]
        public async Task<IActionResult> DeleteAnswerOption(int id)
        {
            try
            {
                await _service.DeleteAnswerOptionAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting quiz question {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("exists/{id}")]
        public async Task<ActionResult<bool>> Exists(int id)
        {
            try
            {
                var exists = await _service.ExistsAsync(id);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if quiz question exists {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
