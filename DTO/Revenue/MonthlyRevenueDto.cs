namespace FirstAidAPI.DTO.Revenue
{
    public class MonthlyRevenueDto
    {
        public int Month { get; set; }
        public string MonthName { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
    }
}