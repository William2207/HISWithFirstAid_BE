using FirstAidAPI.DTO.Revenue;
using FirstAidAPI.Repository;

namespace FirstAidAPI.Service.Implement
{
    public class RevenueService : IRevenueService
    {
        private readonly IOrderRepository _orderRepository;

        public RevenueService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<YearlyRevenueDto> GetCurrentYearRevenueAsync()
        {
            var currentYear = DateTime.Now.Year;
            return await _orderRepository.GetYearlyRevenueAsync(currentYear);
        }

        public async Task<YearlyRevenueDto> GetRevenueByYearAsync(int year)
        {
            if (year < 2000 || year > DateTime.Now.Year)
            {
                throw new ArgumentException("Invalid year");
            }

            return await _orderRepository.GetYearlyRevenueAsync(year);
        }

        public async Task<List<MonthlyRevenueDto>> GetMonthlyRevenueAsync(int year)
        {
            if (year < 2000 || year > DateTime.Now.Year)
            {
                throw new ArgumentException("Invalid year");
            }

            return await _orderRepository.GetMonthlyRevenueAsync(year);
        }
    }
}