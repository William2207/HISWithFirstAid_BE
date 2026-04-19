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
        [AllowAnonymous]
        public async Task<IActionResult> GetDoctorsForLookup()
        {
            var doctors = await _doctorService.GetDoctorsForLookupAsync();
            return Ok(doctors);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableDoctors([FromQuery] int specialtyId, [FromQuery] DateTime date)
        {
            var result = await _doctorService.GetAvailableDoctorsAsync(specialtyId, date);
            return Ok(result);
        }
    }
}
