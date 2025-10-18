using FirstAidAPI.Data;
using FirstAidAPI.DTO;
using FirstAidAPI.Extensions;
using FirstAidAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
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
        public async Task<ActionResult<PagedResult<Technique>>> GetTechniques([FromQuery] int page = 1, [FromQuery] int pageSize = 9,
            [FromQuery] List<string>? difficulties = null, [FromQuery] List<string>? types = null, [FromQuery] string? search = null)

        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 9;
            if (pageSize > 100) pageSize = 100;
            var query = _context.Techniques.AsQueryable();

            // Filter theo difficulties
            if (difficulties != null && difficulties.Any())
            {
                query = query.Where(t => difficulties.Contains(t.Difficulty));
            }

            // Filter theo types
            if (types != null && types.Any())
            {
                query = query.Where(t => types.Contains(t.Type));
            }

            // Filter theo search
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(t =>
                    t.Title.ToLower().Contains(searchLower) ||
                    t.Description.ToLower().Contains(searchLower)
                );
            }

            query = query.OrderBy(t => t.Id);

            var result = await query.ToPagedResultAsync(page, pageSize);
            //Console.WriteLine("du lieu trang technique" + JsonSerializer.Serialize(result));
            return Ok(result);
        }

        // GET: api/techniques/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Technique>> GetTechnique(int id)
        {
            var technique = await _context.Techniques
                .Include(t => t.TechniqueSteps.OrderBy(s => s.StepNumber))
                .FirstOrDefaultAsync(t => t.Id == id);

            if (technique == null)
            {
                return NotFound();
            }

            return technique;
        }

    }
}
