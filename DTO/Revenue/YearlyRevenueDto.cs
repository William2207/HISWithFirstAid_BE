namespace FirstAidAPI.DTO.Revenue
{
    public class YearlyRevenueDto
    {
        public int Year { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<MonthlyRevenueDto> MonthlyRevenues { get; set; } = new();
    }
}