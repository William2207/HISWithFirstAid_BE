using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FirstAidAPI.Data;
using FirstAidAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScenariosController : Controller
    {

        private readonly FirstAidContext _context;

        public ScenariosController(FirstAidContext context)
        {
            _context = context;
        }

        // GET: api/scenarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Scenario>>> GetScenarios()
        {
            return await _context.Scenarios.ToListAsync();
        }

        // GET: api/scenarios/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Scenario>> GetScenario(int id)
        {
            var scenario = await _context.Scenarios
                .Include(s => s.ScenarioSteps)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (scenario == null)
            {
                return NotFound();
            }

            return scenario;
        }
    }
}
