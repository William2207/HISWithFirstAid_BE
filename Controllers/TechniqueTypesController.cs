using FirstAidAPI.DTO.Technique;
using FirstAidAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechniqueTypesController : Controller
    {
        private readonly ITechniqueTypeService _service;
        private readonly ILogger<TechniquesController> _logger;

        public TechniqueTypesController(ITechniqueTypeService service, ILogger<TechniquesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Get all technique types
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TechniqueTypeDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TechniqueTypeDto>>> GetAll()
        {
            var techniqueTypes = await _service.GetAllAsync();
            return Ok(techniqueTypes);
        }

        /// <summary>
        /// Get technique type by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TechniqueTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TechniqueTypeDto>> GetById(int id)
        {
            var techniqueType = await _service.GetByIdAsync(id);
            if (techniqueType == null)
            {
                return NotFound(new { message = $"TechniqueType with ID {id} not found." });
            }
            return Ok(techniqueType);
        }

        /// <summary>
        /// Create new technique type
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(TechniqueTypeDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TechniqueTypeDto>> Create([FromBody] CreateTechniqueTypeDto dto)
        {
            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update technique type
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TechniqueTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TechniqueTypeDto>> Update(int id, [FromBody] UpdateTechniqueTypeDto dto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, dto);
                return Ok(updated);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete technique type
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);
                return Ok(new { message = "Xóa thành công" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Trả về message ngắn gọn thay vì toàn bộ stack trace
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Log lỗi ở đây nếu cần
                _logger.LogError(ex, "Lỗi khi xóa với id {Id}", id);
                return StatusCode(500, new { message = "Đã có lỗi xảy ra" });
            }
        }
    }
}