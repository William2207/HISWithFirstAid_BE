using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IDoctorScheduleRepository
    {
        Task<DoctorSchedule?> GetByDoctorAndDateAsync(int doctorId, DateOnly date);

        Task<List<DoctorSchedule>> GetByMonthAsync(int month, int year);

        Task<List<DoctorSchedule>> GetByDoctorAndMonthAsync(int doctorId, int month, int year);

        Task<List<DoctorSchedule>> GetBySpecialtyAndMonthAsync(int specialtyId, int month, int year);

        Task<bool> ExistsForMonthAsync(int month, int year);

        Task<bool> ExistsForSpecialtyAndMonthAsync(int specialtyId, int month, int year);

        Task AddRangeAsync(List<DoctorSchedule> schedules);

        Task DeleteByMonthAsync(int month, int year);

        Task DeleteBySpecialtyAndMonthAsync(int specialtyId, int month, int year);
    }
}
