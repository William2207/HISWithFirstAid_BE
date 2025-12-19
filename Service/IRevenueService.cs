using FirstAidAPI.DTO.Revenue;

namespace FirstAidAPI.Service
{
    public interface IRevenueService
    {
        Task<YearlyRevenueDto> GetCurrentYearRevenueAsync();

        Task<YearlyRevenueDto> GetRevenueByYearAsync(int year);

        Task<List<MonthlyRevenueDto>> GetMonthlyRevenueAsync(int year);
    }
}