using FirstAidAPI.DTO;
using FirstAidAPI.Models;
using FirstAidAPI.Service;
using FirstAidAPI.Service.Implement;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

// Các using không cần thiết nữa: Microsoft.EntityFrameworkCore, FirstAidAPI.Data, FirstAidAPI.Extensions

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScenariosController : ControllerBase // Dùng ControllerBase cho API
    {
        // Thay thế DbContext bằng IScenarioService
        private readonly IScenarioService _service;

        private readonly ILogger<ScenariosController> _logger;

        public ScenariosController(IScenarioService service, ILogger<ScenariosController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: api/scenarios
        [HttpGet]
        public async Task<ActionResult<PagedResult<Scenario>>> GetScenarios(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 9,
            [FromQuery] List<string>? difficulties = null,
            [FromQuery] List<string>? types = null,
            [FromQuery] string? search = null)
        {
            // Toàn bộ logic đã được chuyển đi, chỉ còn một lời gọi service duy nhất
            var result = await _service.GetScenariosAsync(page, pageSize, difficulties, types, search);
            return Ok(result);
        }

        // GET: api/scenarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Scenario>> GetScenario(int id)
        {
            var scenario = await _service.GetScenarioByIdAsync(id);

            if (scenario == null)
            {
                return NotFound();
            }

            return Ok(scenario);
        }

        [HttpGet("all")]
        [ProducesResponseType(typeof(IEnumerable<ScenarioDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ScenarioDto>>> GetAllScenarios()
        {
            try
            {
                var scenarios = await _service.GetAllScenariosAsync();
                return Ok(scenarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all scenarios");
                return StatusCode(500, "An error occurred while retrieving scenarios");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ScenarioDetailDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ScenarioDetailDto>> CreateScenario([FromBody] CreateScenarioDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdScenario = await _service.CreateScenarioAsync(createDto);
                return CreatedAtAction(
                    nameof(GetScenario),
                    new { id = createdScenario.Id },
                    createdScenario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating scenario");
                return StatusCode(500, "An error occurred while creating the scenario");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ScenarioDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ScenarioDetailDto>> UpdateScenario(int id, [FromBody] UpdateScenarioDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedScenario = await _service.UpdateScenarioAsync(id, updateDto);
                return Ok(updatedScenario);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Scenario with ID {ScenarioId} not found", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating scenario with ID {ScenarioId}", id);
                return StatusCode(500, "An error occurred while updating the scenario");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteScenario(int id)
        {
            try
            {
                await _service.DeleteScenarioAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Scenario with ID {ScenarioId} not found", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting scenario with ID {ScenarioId}", id);
                return StatusCode(500, "An error occurred while deleting the scenario");
            }
        }
    }
}