using FirstAidAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet("lookup")]
        [AllowAnonymous] // Assuming lookups might be needed generally, or keep if required
        public async Task<IActionResult> GetDoctorsForLookup()
        {
            var doctors = await _doctorService.GetDoctorsForLookupAsync();
            return Ok(doctors);
        }
    }
}
