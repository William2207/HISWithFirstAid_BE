using FirstAidAPI.DTO.MedicalRecord;

namespace FirstAidAPI.Service
{
    public interface IMedicalRecordService
    {
        Task<MedicalRecordDTO> CreateMedicalRecordAsync(int doctorId, CreateMedicalRecordRequest request);

        Task<MedicalRecordDTO> GetMedicalRecordByIdAsync(int id);

        Task<MedicalRecordDTO> GetMedicalRecordByAppointmentIdAsync(int appointmentId);

        Task<IEnumerable<MedicalRecordDTO>> GetMedicalRecordsByPatientAsync(int patientId);

        Task<IEnumerable<MedicalRecordDTO>> GetMedicalRecordsByUserIdAsync(int userId);

        Task<MedicalRecordDTO> UpdateMedicalRecordAsync(int id, int doctorId, UpdateMedicalRecordRequest request);
    }
}
