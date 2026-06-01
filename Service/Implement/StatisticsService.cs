using FirstAidAPI.Data;
using FirstAidAPI.DTO.Statistics;
using FirstAidAPI.Enums;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI.Service.Implement
{
    public class StatisticsService : IStatisticsService
    {
        private readonly FirstAidContext _context;
        private static readonly string[] MonthNames = {
            "Th1", "Th2", "Th3", "Th4", "Th5", "Th6",
            "Th7", "Th8", "Th9", "Th10", "Th11", "Th12"
        };

        public StatisticsService(FirstAidContext context)
        {
            _context = context;
        }

        public async Task<ReceptionistDashboardDto> GetReceptionistDashboardAsync()
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            var waitingPatients = await _context.Appointments
                .CountAsync(a => a.Status == AppointmentStatus.Registered
                              && a.AppointmentDateTime >= today
                              && a.AppointmentDateTime < tomorrow);

            var totalToday = await _context.Appointments
                .CountAsync(a => a.AppointmentDateTime >= today && a.AppointmentDateTime < tomorrow);

            var completedOrInProgress = await _context.Appointments
                .CountAsync(a => (a.Status == AppointmentStatus.Completed || a.Status == AppointmentStatus.In_Progress)
                              && a.AppointmentDateTime >= today
                              && a.AppointmentDateTime < tomorrow);

            return new ReceptionistDashboardDto
            {
                WaitingPatients = waitingPatients,
                TotalAppointmentsToday = totalToday,
                RemainingAppointments = totalToday - completedOrInProgress
            };
        }

        public async Task<List<DailyPatientCountDto>> GetDailyPatientCountsAsync(int days = 30)
        {
            var endDate = DateTime.UtcNow.Date;
            var startDate = endDate.AddDays(-days + 1);
            var endDateTime = endDate.AddDays(1);

            var counts = await _context.Appointments
                .Where(a => a.AppointmentDateTime >= startDate && a.AppointmentDateTime < endDateTime)
                .GroupBy(a => a.AppointmentDateTime.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToListAsync();

            var result = new List<DailyPatientCountDto>();
            for (int i = 0; i < days; i++)
            {
                var date = startDate.AddDays(i);
                var found = counts.FirstOrDefault(c => c.Date == date);
                result.Add(new DailyPatientCountDto
                {
                    Date = date.ToString("dd/MM"),
                    Count = found?.Count ?? 0
                });
            }
            return result;
        }

        public async Task<List<MonthlyPatientCountDto>> GetMonthlyPatientCountsAsync(int year)
        {
            var counts = await _context.Appointments
                .Where(a => a.AppointmentDateTime.Year == year)
                .GroupBy(a => a.AppointmentDateTime.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToListAsync();

            var result = new List<MonthlyPatientCountDto>();
            for (int m = 1; m <= 12; m++)
            {
                var found = counts.FirstOrDefault(c => c.Month == m);
                result.Add(new MonthlyPatientCountDto
                {
                    Month = m,
                    MonthName = MonthNames[m - 1],
                    Count = found?.Count ?? 0
                });
            }
            return result;
        }

        public async Task<List<YearlyPatientCountDto>> GetYearlyPatientCountsAsync()
        {
            var currentYear = DateTime.UtcNow.Year;
            var startYear = currentYear - 4;

            var counts = await _context.Appointments
                .Where(a => a.AppointmentDateTime.Year >= startYear && a.AppointmentDateTime.Year <= currentYear)
                .GroupBy(a => a.AppointmentDateTime.Year)
                .Select(g => new { Year = g.Key, Count = g.Count() })
                .ToListAsync();

            var result = new List<YearlyPatientCountDto>();
            for (int y = startYear; y <= currentYear; y++)
            {
                var found = counts.FirstOrDefault(c => c.Year == y);
                result.Add(new YearlyPatientCountDto
                {
                    Year = y,
                    Count = found?.Count ?? 0
                });
            }
            return result;
        }
    }
}
