using FirstAidAPI.DTO.Appointment;

namespace FirstAidAPI.Service
{
    public interface IAppointmentService
    {
        Task<AppointmentDTO> CreateAppointmentAsync(int creatorId, CreateAppointmentRequest request);

        Task<IEnumerable<AppointmentDTO>> GetWaitingAppointmentsByDoctorAsync(int doctorId);

        Task<IEnumerable<AppointmentDTO>> GetAppointmentsByDoctorAndDateAsync(int doctorId, DateTime date);

        Task<AppointmentDTO> GetAppointmentByIdAsync(int id);

        /// <summary>
        /// Doctor gọi bệnh nhân vào khám: chuyển Status → In_Progress và auto-tạo MedicalRecord trống.
        /// </summary>
        Task<AppointmentDTO> StartAppointmentAsync(int appointmentId, int doctorId);

        Task<AppointmentDTO> CompleteAppointmentAsync(int appointmentId);

        Task<IEnumerable<AppointmentDTO>> GetAppointmentsByPatientAsync(int patientId);

        Task<IEnumerable<AppointmentDTO>> GetAppointmentsByUserIdAsync(int userId);

        Task<IEnumerable<AppointmentDTO>> GetCompletedAppointmentsAsync();

        Task CancelAppointmentAsync(int appointmentId);
    }
}
