using FirstAidAPI.DTO.Doctor;

namespace FirstAidAPI.Service
{
    public interface IScheduleService
    {
        Task GenerateMonthlyScheduleAsync(int month, int year);

        Task<List<DoctorScheduleDto>> GetMonthlyScheduleAsync(int month, int year);

        Task<List<DoctorScheduleDto>> GetDoctorScheduleAsync(int doctorId, int month, int year);
    }
}
