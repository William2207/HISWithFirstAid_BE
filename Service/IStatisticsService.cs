using FirstAidAPI.DTO.Statistics;

namespace FirstAidAPI.Service
{
    public interface IStatisticsService
    {
        Task<ReceptionistDashboardDto> GetReceptionistDashboardAsync();
        Task<AdminDashboardDto> GetAdminDashboardAsync();
        Task<List<DailyPatientCountDto>> GetDailyPatientCountsAsync(int days);
        Task<List<MonthlyPatientCountDto>> GetMonthlyPatientCountsAsync(int year);
        Task<List<YearlyPatientCountDto>> GetYearlyPatientCountsAsync();
    }
}
