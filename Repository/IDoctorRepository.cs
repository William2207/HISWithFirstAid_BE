using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IDoctorRepository
    {
        Task<Doctor> AddAsync(Doctor doctor);

        Task UpdateAsync(Doctor doctor);

        Task DeleteAsync(int id);

        Task<Doctor?> GetByIdAsync(int id);

        Task<Doctor?> GetByUserIdAsync(int userId);

        Task<List<Doctor>> GetAllAsync();

        Task<bool> ExistsByUserIdAsync(int userId);

        Task<List<Doctor>> GetDoctorsBySpecialtyForBookingAsync(int specialtyId);

        Task<Clinic?> GetDefaultClinicBySpecialtyAsync(int specialtyId);

        Task<DoctorSchedule?> GetScheduleAsync(int doctorId, DayOfWeek dayOfWeek, TimeSpan timeOfDay);

        Task UpdateScheduleAsync(DoctorSchedule schedule);
    }
}
