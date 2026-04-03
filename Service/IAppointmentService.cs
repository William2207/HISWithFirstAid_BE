using FirstAidAPI.DTO.Appointment;

namespace FirstAidAPI.Service
{
    public interface IAppointmentService
    {
        Task<AppointmentDTO> CreateAppointmentAsync(int creatorId, CreateAppointmentRequest request);

        Task<IEnumerable<AppointmentDTO>> GetWaitingAppointmentsByDoctorAsync(int doctorId);

        Task<AppointmentDTO> GetAppointmentByIdAsync(int id);

        Task<AppointmentDTO> CompleteAppointmentAsync(int appointmentId);
    }
}
