using FirstAidAPI.DTO;
using FirstAidAPI.Models;
using FirstAidAPI.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechniquesController : ControllerBase
    {
        // Thay thế DbContext bằng ITechniqueService
        private readonly ITechniqueService _service;

        public TechniquesController(ITechniqueService service)
        {
            _service = service;
        }

        // GET: api/techniques
        [HttpGet]
        public async Task<ActionResult<PagedResult<Technique>>> GetTechniques(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 9,
            [FromQuery] List<string>? difficulties = null,
            [FromQuery] List<string>? types = null,
            [FromQuery] string? search = null)
        {
            // Toàn bộ logic phức tạp đã được chuyển đi, chỉ còn một lời gọi service
            var result = await _service.GetTechniquesAsync(page, pageSize, difficulties, types, search);
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
    }
}