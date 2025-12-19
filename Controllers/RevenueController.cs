using FirstAidAPI.DTO.Revenue;
using FirstAidAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace FirstAidAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RevenueController : Controller
    {
        private readonly IRevenueService _revenueService;

        public RevenueController(IRevenueService revenueService)
        {
            _revenueService = revenueService;
        }

        /// <summary>
        /// Lấy doanh thu năm hiện tại (tổng + chi tiết 12 tháng)
        /// </summary>
        [HttpGet("current-year")]
        public async Task<ActionResult<YearlyRevenueDto>> GetCurrentYearRevenue()
        {
            try
            {
                var result = await _revenueService.GetCurrentYearRevenueAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving revenue data", error = ex.Message });
            }
        }

        /// <summary>
        /// Lấy doanh thu theo năm cụ thể
        /// </summary>
        [HttpGet("year/{year}")]
        public async Task<ActionResult<YearlyRevenueDto>> GetRevenueByYear(int year)
        {
            try
            {
                var result = await _revenueService.GetRevenueByYearAsync(year);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving revenue data", error = ex.Message });
            }
        }

        /// <summary>
        /// Lấy doanh thu chi tiết 12 tháng theo năm
        /// </summary>
        [HttpGet("monthly/{year}")]
        public async Task<ActionResult<List<MonthlyRevenueDto>>> GetMonthlyRevenue(int year)
        {
            try
            {
                var result = await _revenueService.GetMonthlyRevenueAsync(year);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving revenue data", error = ex.Message });
            }
        }
    }
}