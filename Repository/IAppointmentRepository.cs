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
    }
}
