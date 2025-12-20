using FirstAidAPI.Data;
using FirstAidAPI.DTO.Revenue;
using FirstAidAPI.Enums;
using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI.Repository.Implement
{
    public class OrderRepository : IOrderRepository
    {
        private readonly FirstAidContext _context;

        public OrderRepository(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems) // <--- Bắt buộc phải có dòng này để load OrderItems
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> CreateAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> ExistAsync(int id)
        {
            return await _context.Orders.AnyAsync(o => o.Id == id);
        }

        public async Task<Order?> GetByOrderNumberAsync(string orderNumber)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId)
        {
            return await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
        }

        public async Task<Order?> GetByIdWithItemsAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<MonthlyRevenueDto>> GetMonthlyRevenueAsync(int year)
        {
            var monthlyData = await _context.Orders
                .Where(o => o.CompletedAt.HasValue
                            && o.CompletedAt.Value.Year == year
                            && o.PaymentStatus == PaymentStatus.Completed)
                .GroupBy(o => o.CompletedAt!.Value.Month)
                .Select(g => new MonthlyRevenueDto
                {
                    Month = g.Key,
                    Revenue = g.Sum(o => o.TotalAmount)
                })
                .OrderBy(x => x.Month)
                .ToListAsync();

            // Tạo danh sách đầy đủ 12 tháng (tháng nào không có doanh thu = 0)
            var allMonths = Enumerable.Range(1, 12).Select(month => new MonthlyRevenueDto
            {
                Month = month,
                MonthName = new DateTime(year, month, 1).ToString("MMMM"),
                Revenue = monthlyData.FirstOrDefault(m => m.Month == month)?.Revenue ?? 0
            }).ToList();

            return allMonths;
        }

        public async Task<YearlyRevenueDto> GetYearlyRevenueAsync(int year)
        {
            var monthlyRevenues = await GetMonthlyRevenueAsync(year);

            return new YearlyRevenueDto
            {
                Year = year,
                TotalRevenue = monthlyRevenues.Sum(m => m.Revenue),
                MonthlyRevenues = monthlyRevenues
            };
        }
    }
}