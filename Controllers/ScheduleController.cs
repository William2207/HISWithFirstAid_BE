using FirstAidAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace FirstAidAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : Controller
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromQuery] int month, [FromQuery] int year)
        {
            await _scheduleService.GenerateMonthlyScheduleAsync(month, year);
            return Ok("Xếp lịch thành công!");
        }

        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthly([FromQuery] int month, [FromQuery] int year)
        {
            var result = await _scheduleService.GetMonthlyScheduleAsync(month, year);
            return Ok(result);
        }

        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetDoctorSchedule(
            int doctorId,
            [FromQuery] int month,
            [FromQuery] int year)
        {
            var result = await _scheduleService.GetDoctorScheduleAsync(doctorId, month, year);
            return Ok(result);
        }
    }
}
