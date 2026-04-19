using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IMedicalRecordRepository
    {
        Task<MedicalRecord> CreateAsync(MedicalRecord medicalRecord);

        Task<MedicalRecord?> GetByIdAsync(int id);

        Task<MedicalRecord?> GetByAppointmentIdAsync(int appointmentId);

        Task<List<MedicalRecord>> GetByPatientIdAsync(int patientId);

        Task<MedicalRecord> UpdateAsync(MedicalRecord medicalRecord);

        Task<bool> ExistsByAppointmentIdAsync(int appointmentId);
    }
}
