using FirstAidAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SpecialtiesController : ControllerBase
    {
        private readonly ISpecialtyService _specialtyService;

        public SpecialtiesController(ISpecialtyService specialtyService)
        {
            _specialtyService = specialtyService;
        }

        [HttpGet("lookup")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSpecialtiesForLookup()
        {
            var specialties = await _specialtyService.GetSpecialtiesForLookupAsync();
            return Ok(specialties);
        }
    }
}
