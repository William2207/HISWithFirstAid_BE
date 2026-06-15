namespace FirstAidAPI.DTO.Revenue
{
    public class YearlyRevenueDto
    {
        public int Year { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalCourseRevenue { get; set; }
        public decimal TotalHospitalRevenue { get; set; }
        public List<MonthlyRevenueDto> MonthlyRevenues { get; set; } = new();
    }
}