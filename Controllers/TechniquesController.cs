using FirstAidAPI.DTO;
using FirstAidAPI.Models;
using FirstAidAPI.Service;
using FirstAidAPI.Service.Implement;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using FirstAidAPI.Extensions;
using FirstAidAPI.DTO.Technique;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechniquesController : ControllerBase
    {
        // Thay thế DbContext bằng ITechniqueService
        private readonly ITechniqueService _service;

        private readonly ILogger<TechniquesController> _logger;

        public TechniquesController(ITechniqueService service, ILogger<TechniquesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: api/techniques
        [HttpGet]
        public async Task<ActionResult<PagedResult<Technique>>> GetTechniques(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 9,
            [FromQuery] List<string>? difficulties = null,
            [FromQuery] List<int>? typeIds = null,
            [FromQuery] string? search = null)
        {
            // Toàn bộ logic phức tạp đã được chuyển đi, chỉ còn một lời gọi service
            var result = await _service.GetTechniquesAsync(page, pageSize, difficulties, typeIds, search);
            return Ok(result);
        }

        // GET: api/techniques/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Technique>> GetTechnique(int id)
        {
            var technique = await _service.GetTechniqueByIdAsync(id);

            if (technique == null)
            {
                return NotFound();
            }
            return Ok(technique);
        }

        [HttpPost]
        [ProducesResponseType(typeof(TechniqueDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TechniqueDto>> Create([FromBody] CreateTechniqueDto dto)
        {
            try
            {
                var technique = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetTechnique), new { id = technique.Id }, technique);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating technique");
                return StatusCode(500, new { message = "An error occurred while creating the technique" });
            }
        }

        /// <summary>
        /// Update an existing technique (partial update)
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TechniqueDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TechniqueDto>> Update(int id, [FromBody] UpdateTechniqueDto dto)
        {
            try
            {
                var technique = await _service.UpdateAsync(id, dto);

                if (technique == null)
                {
                    return NotFound(new { message = $"Technique with ID {id} not found" });
                }

                return Ok(technique);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating technique {TechniqueId}", id);
                return StatusCode(500, new { message = "An error occurred while updating the technique" });
            }
        }

        [HttpPut("step/{id}")]
        [ProducesResponseType(typeof(TechniqueStepDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<TechniqueStepDto>>> UpdateSteps(int id, [FromBody] UpdateTechniqueStepsDto dto)
        {
            try
            {
                var steps = await _service.UpdateStepsAsync(id, dto);
                if (steps == null || steps.Count == 0)
                {
                    return NotFound(new { message = $"Technique with ID {id} not found or no steps to update" });
                }
                return Ok(steps);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating steps for technique {TechniqueId}", id);
                return StatusCode(500, new { message = "An error occurred while updating the technique steps" });
            }
        }

        /// <summary>
        /// Delete a technique
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new { message = $"Technique with ID {id} not found" });
            }

            return NoContent();
        }

        /// <summary>
        /// Check if technique exists
        /// </summary>
        [HttpHead("{id}")]
        [HttpGet("{id}/exists")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Exists(int id)
        {
            var exists = await _service.ExistsAsync(id);
            return exists ? Ok(new { exists = true }) : NotFound(new { exists = false });
        }
    }
}