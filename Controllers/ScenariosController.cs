using FirstAidAPI.DTO;
using FirstAidAPI.Models;
using FirstAidAPI.Service;
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

        public ScenariosController(IScenarioService service)
        {
            _service = service;
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
    }
}