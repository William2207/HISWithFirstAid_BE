using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IAppointmentRepository
    {
        Task<Appointment> AddAsync(Appointment appointment);

        Task<Appointment> UpdateAsync(Appointment appointment);

        Task<Appointment> DeleteAsync(Appointment appointment);

        Task<Appointment?> GetByIdAsync(int id);

        Task<IEnumerable<Appointment>> GetWaitingAppointmentsByDoctorAsync(int doctorId);

        Task<IEnumerable<Appointment>> GetAppointmentsByDoctorAndDateAsync(int doctorId, DateTime date);

        Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId);

        Task<IEnumerable<Appointment>> GetCompletedAppointmentsAsync();
        
        Task<bool> ExistsOverlapAsync(int patientId, int specialtyId, DateTime dateTime);

        /// <summary>
        /// Đếm số lượng appointment theo bác sĩ, thời gian và loại, đồng thời lock các row này (FOR UPDATE)
        /// để tránh race condition khi nhiều request đặt slot cùng lúc.
        /// </summary>
        Task<int> GetSlotCountWithLockAsync(int doctorId, DateTime appointmentDateTime, int appointmentType);
    }
}
