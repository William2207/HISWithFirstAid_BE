namespace FirstAidAPI.DTO.Statistics
{
    public class ReceptionistDashboardDto
    {
        public int WaitingPatients { get; set; }
        public int RemainingAppointments { get; set; }
        public int TotalAppointmentsToday { get; set; }
    }

    public class AdminDashboardDto
    {
        public int TotalPatients { get; set; }
        public int TotalAppointmentsToday { get; set; }
    }

    public class DailyPatientCountDto
    {
        public string Date { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class MonthlyPatientCountDto
    {
        public int Month { get; set; }
        public string MonthName { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class YearlyPatientCountDto
    {
        public int Year { get; set; }
        public int Count { get; set; }
    }
}
