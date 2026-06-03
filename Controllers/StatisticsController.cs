using FirstAidAPI.DTO.Statistics;
using FirstAidAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstAidAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Receptionist,Admin")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        /// <summary>
        /// Lấy dữ liệu tổng quan cho Receptionist (số bệnh nhân chờ, lịch hẹn còn lại)
        /// </summary>
        [HttpGet("receptionist-dashboard")]
        public async Task<ActionResult<ReceptionistDashboardDto>> GetReceptionistDashboard()
        {
            try
            {
                var result = await _statisticsService.GetReceptionistDashboardAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving dashboard data", error = ex.Message });
            }
        }

        /// <summary>
        /// Số bệnh nhân hàng ngày trong N ngày gần nhất (mặc định 30 ngày)
        /// </summary>
        [HttpGet("patients/daily")]
        public async Task<ActionResult<List<DailyPatientCountDto>>> GetDailyPatientCounts([FromQuery] int days = 30)
        {
            try
            {
                if (days < 1 || days > 365) days = 30;
                var result = await _statisticsService.GetDailyPatientCountsAsync(days);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving daily patient data", error = ex.Message });
            }
        }

        /// <summary>
        /// Số bệnh nhân theo từng tháng trong một năm
        /// </summary>
        [HttpGet("patients/monthly/{year}")]
        public async Task<ActionResult<List<MonthlyPatientCountDto>>> GetMonthlyPatientCounts(int year)
        {
            try
            {
                if (year < 2020 || year > DateTime.UtcNow.Year)
                    return BadRequest(new { message = "Invalid year" });

                var result = await _statisticsService.GetMonthlyPatientCountsAsync(year);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving monthly patient data", error = ex.Message });
            }
        }

        /// <summary>
        /// Số bệnh nhân theo từng năm (5 năm gần nhất)
        /// </summary>
        [HttpGet("patients/yearly")]
        public async Task<ActionResult<List<YearlyPatientCountDto>>> GetYearlyPatientCounts()
        {
            try
            {
                var result = await _statisticsService.GetYearlyPatientCountsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving yearly patient data", error = ex.Message });
            }
        }
    }
}
