using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface INurseScheduleRepository
    {
        Task<NurseSchedule?> GetByNurseAndDateAsync(int nurseId, DateOnly date);
        Task<List<NurseSchedule>> GetByMonthAsync(int month, int year);
        Task<List<NurseSchedule>> GetByNurseAndMonthAsync(int nurseId, int month, int year);
        Task<List<NurseSchedule>> GetBySpecialtyAndMonthAsync(int specialtyId, int month, int year);
        Task<bool> ExistsForMonthAsync(int month, int year);
        Task<bool> ExistsForSpecialtyAndMonthAsync(int specialtyId, int month, int year);
        Task AddRangeAsync(List<NurseSchedule> schedules);
        Task DeleteByMonthAsync(int month, int year);
        Task DeleteBySpecialtyAndMonthAsync(int specialtyId, int month, int year);
    }
}
