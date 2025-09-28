using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirstAidAPI.Data;
using FirstAidAPI.Models;
namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechniquesController : Controller
    {
        private readonly FirstAidContext _context;

        public TechniquesController(FirstAidContext context)
        {
            _context = context;
        }

        // GET: api/techniques
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Technique>>> GetTechniques()
        {
            return await _context.Techniques.ToListAsync();
        }

        // GET: api/techniques/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Technique>> GetTechnique(int id)
        {
            var technique = await _context.Techniques
                .Include(t => t.ScenarioSteps)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (technique == null)
            {
                return NotFound();
            }

            return technique;
        }

    }
}
