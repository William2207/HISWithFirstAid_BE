using FirstAidAPI.Data;
using FirstAidAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : Controller
    {
        private readonly FirstAidContext _context;

        public QuizController(FirstAidContext context)
        {
            _context = context;
        }

        // GET: api/quiz/technique/1
        [HttpGet("technique/{techniqueId}")]
        public async Task<ActionResult<IEnumerable<QuizQuestion>>> GetQuizQuestionsByTechnique(int techniqueId)
        {
            var questions = await _context.QuizQuestions
                .Where(q => q.TechniqueId == techniqueId)
                .Include(q => q.AnswerOptions)
                .ToListAsync();

            return questions;
        }
    }
}