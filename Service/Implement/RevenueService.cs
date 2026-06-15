using FirstAidAPI.DTO.Revenue;
using FirstAidAPI.Repository;

namespace FirstAidAPI.Service.Implement
{
    public class RevenueService : IRevenueService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IInvoiceRepository _invoiceRepository;

        public RevenueService(IOrderRepository orderRepository, IInvoiceRepository invoiceRepository)
        {
            _orderRepository = orderRepository;
            _invoiceRepository = invoiceRepository;
        }

        public async Task<YearlyRevenueDto> GetCurrentYearRevenueAsync()
        {
            var currentYear = DateTime.Now.Year;
            return await GetRevenueByYearAsync(currentYear);
        }

        public async Task<YearlyRevenueDto> GetRevenueByYearAsync(int year)
        {
            if (year < 2000 || year > DateTime.Now.Year)
            {
                throw new ArgumentException("Invalid year");
            }

            var monthlyRevenues = await GetMonthlyRevenueAsync(year);

            return new YearlyRevenueDto
            {
                Year = year,
                TotalCourseRevenue = monthlyRevenues.Sum(m => m.CourseRevenue),
                TotalHospitalRevenue = monthlyRevenues.Sum(m => m.HospitalRevenue),
                TotalRevenue = monthlyRevenues.Sum(m => m.Revenue),
                MonthlyRevenues = monthlyRevenues
            };
        }

        public async Task<List<MonthlyRevenueDto>> GetMonthlyRevenueAsync(int year)
        {
            if (year < 2000 || year > DateTime.Now.Year)
            {
                throw new ArgumentException("Invalid year");
            }

            var orderMonthlyData = await _orderRepository.GetMonthlyRevenueAsync(year);
            var invoiceMonthlyData = await _invoiceRepository.GetMonthlyRevenueAsync(year);

            var combinedMonths = Enumerable.Range(1, 12).Select(month => {
                var orderData = orderMonthlyData.FirstOrDefault(m => m.Month == month);
                var invoiceData = invoiceMonthlyData.FirstOrDefault(m => m.Month == month);
                
                decimal courseRev = orderData?.CourseRevenue ?? 0;
                decimal hospitalRev = invoiceData?.HospitalRevenue ?? 0;

                return new MonthlyRevenueDto
                {
                    Month = month,
                    MonthName = new DateTime(year, month, 1).ToString("MMMM"),
                    CourseRevenue = courseRev,
                    HospitalRevenue = hospitalRev,
                    Revenue = courseRev + hospitalRev
                };
            }).ToList();

            return combinedMonths;
        }
    }
}
