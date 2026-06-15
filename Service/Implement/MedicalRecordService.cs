using FirstAidAPI.DTO.MedicalRecord;
using FirstAidAPI.Exceptions;
using FirstAidAPI.Models;
using FirstAidAPI.Repository;

namespace FirstAidAPI.Service.Implement
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;

        public MedicalRecordService(
            IMedicalRecordRepository medicalRecordRepository,
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
        }

        public async Task<MedicalRecordDTO> CreateMedicalRecordAsync(int doctorId, CreateMedicalRecordRequest request)
        {
            // Kiểm tra appointment tồn tại và thuộc về doctor này
            var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId);
            if (appointment == null)
                throw new NotFoundException($"Không tìm thấy lịch hẹn với ID {request.AppointmentId}");

            if (appointment.DoctorId != doctorId)
                throw new BusinessException("Bác sĩ không có quyền tạo bệnh án cho lịch hẹn này.");

            // Kiểm tra xem bệnh án đã tồn tại cho lịch hẹn này chưa
            if (await _medicalRecordRepository.ExistsByAppointmentIdAsync(request.AppointmentId))
                throw new BusinessException("Lịch hẹn này đã có hồ sơ bệnh án.");

            var medicalRecord = new MedicalRecord
            {
                AppointmentId = request.AppointmentId,
                DoctorId = doctorId,
                PatientId = appointment.PatientId,   // ← fix: set PatientId từ appointment
                ChiefComplaint = request.ChiefComplaint,
                MedicalHistory = request.MedicalHistory,
                FamilyHistory = request.FamilyHistory,
                Symptoms = request.Symptoms,
                PhysicalExamination = request.PhysicalExamination,
                DiagnosisName = request.DiagnosisName,
                DiagnosisNotes = request.DiagnosisNotes,
                Prescription = request.Prescription,
                TreatmentPlan = request.TreatmentPlan,
                FollowUpInstructions = request.FollowUpInstructions,
                NextAppointmentDate = request.NextAppointmentDate,
                GeneralNotes = request.GeneralNotes,
                IsHospitalized = request.IsHospitalized,
                CreatedAt = DateTime.UtcNow
            };

            if (request.VitalSigns != null)
            {
                medicalRecord.VitalSigns = new VitalSign
                {
                    NurseId = request.VitalSigns.NurseId,
                    BloodPressure = request.VitalSigns.BloodPressure,
                    HeartRate = request.VitalSigns.HeartRate,
                    Temperature = request.VitalSigns.Temperature,
                    RespiratoryRate = request.VitalSigns.RespiratoryRate,
                    SpO2 = request.VitalSigns.SpO2,
                    Weight = request.VitalSigns.Weight,
                    Height = request.VitalSigns.Height,
                    RecordedAt = DateTime.UtcNow
                };
            }

            var createdRecord = await _medicalRecordRepository.CreateAsync(medicalRecord);
            return await GetMedicalRecordByIdAsync(createdRecord.Id);
        }

        public async Task<MedicalRecordDTO> GetMedicalRecordByIdAsync(int id)
        {
            var record = await _medicalRecordRepository.GetByIdAsync(id);
            if (record == null)
            {
                throw new NotFoundException($"Không tìm thấy bệnh án với ID {id}");
            }
            return MapToDTO(record);
        }

        public async Task<MedicalRecordDTO> GetMedicalRecordByAppointmentIdAsync(int appointmentId)
        {
            var record = await _medicalRecordRepository.GetByAppointmentIdAsync(appointmentId);
            if (record == null)
            {
                throw new NotFoundException($"Không tìm thấy bệnh án cho lịch hẹn {appointmentId}");
            }
            return MapToDTO(record);
        }

        public async Task<IEnumerable<MedicalRecordDTO>> GetMedicalRecordsByPatientAsync(int patientId)
        {
            var records = await _medicalRecordRepository.GetByPatientIdAsync(patientId);
            return records.Select(MapToDTO);
        }

        public async Task<IEnumerable<MedicalRecordDTO>> GetMedicalRecordsByUserIdAsync(int userId)
        {
            var patient = await _patientRepository.GetByUserIdAsync(userId);
            if (patient == null)
            {
                throw new NotFoundException("Không tìm thấy hồ sơ bệnh nhân.");
            }

            return await GetMedicalRecordsByPatientAsync(patient.Id);
        }

        public async Task<MedicalRecordDTO> UpdateMedicalRecordAsync(int id, int doctorId, UpdateMedicalRecordRequest request)
        {
            var record = await _medicalRecordRepository.GetByIdAsync(id);
            if (record == null)
            {
                throw new NotFoundException($"Không tìm thấy bệnh án với ID {id}");
            }

            if (record.DoctorId != doctorId)
            {
                throw new BusinessException("Không có quyền cập nhật bệnh án của bác sĩ khác");
            }

            record.ChiefComplaint = request.ChiefComplaint ?? record.ChiefComplaint;
            record.MedicalHistory = request.MedicalHistory ?? record.MedicalHistory;
            record.FamilyHistory = request.FamilyHistory ?? record.FamilyHistory;
            record.Symptoms = request.Symptoms ?? record.Symptoms;
            record.PhysicalExamination = request.PhysicalExamination ?? record.PhysicalExamination;
            record.DiagnosisName = request.DiagnosisName ?? record.DiagnosisName;
            record.DiagnosisNotes = request.DiagnosisNotes ?? record.DiagnosisNotes;
            record.Prescription = request.Prescription ?? record.Prescription;
            record.TreatmentPlan = request.TreatmentPlan ?? record.TreatmentPlan;
            record.FollowUpInstructions = request.FollowUpInstructions ?? record.FollowUpInstructions;
            record.NextAppointmentDate = request.NextAppointmentDate.HasValue
             ? DateTime.SpecifyKind(request.NextAppointmentDate.Value, DateTimeKind.Utc)
             : record.NextAppointmentDate;
            record.GeneralNotes = request.GeneralNotes ?? record.GeneralNotes;
            record.IsHospitalized = request.IsHospitalized; // Trực tiếp gán bool

            if (request.VitalSigns != null)
            {
                record.VitalSigns = new VitalSign
                {
                    NurseId = record.VitalSigns.NurseId,
                    BloodPressure = record.VitalSigns.BloodPressure,
                    HeartRate = record.VitalSigns.HeartRate,
                    Temperature = record.VitalSigns.Temperature,
                    RespiratoryRate = record.VitalSigns.RespiratoryRate,
                    SpO2 = record.VitalSigns.SpO2,
                    Weight = record.VitalSigns.Weight,
                    Height = record.VitalSigns.Height,
                    RecordedAt = DateTime.UtcNow
                };
            }

            var updatedRecord = await _medicalRecordRepository.UpdateAsync(record);
            return MapToDTO(updatedRecord);
        }

        private MedicalRecordDTO MapToDTO(MedicalRecord record)
        {
            return new MedicalRecordDTO
            {
                Id = record.Id,
                AppointmentId = record.AppointmentId,
                DoctorId = record.DoctorId,
                DoctorName = record.Doctor?.User?.FullName,
                ChiefComplaint = record.ChiefComplaint,
                MedicalHistory = record.MedicalHistory,
                FamilyHistory = record.FamilyHistory,
                Symptoms = record.Symptoms,
                PhysicalExamination = record.PhysicalExamination,
                DiagnosisName = record.DiagnosisName,
                DiagnosisNotes = record.DiagnosisNotes,
                Prescription = record.Prescription,
                TreatmentPlan = record.TreatmentPlan,
                FollowUpInstructions = record.FollowUpInstructions,
                NextAppointmentDate = record.NextAppointmentDate,
                GeneralNotes = record.GeneralNotes,
                IsHospitalized = record.IsHospitalized,
                CreatedAt = record.CreatedAt,
                VitalSigns = record.VitalSigns != null ? new VitalSignDTO
                {
                    Id = record.VitalSigns.Id,
                    BloodPressure = record.VitalSigns.BloodPressure,
                    HeartRate = record.VitalSigns.HeartRate,
                    Temperature = record.VitalSigns.Temperature,
                    RespiratoryRate = record.VitalSigns.RespiratoryRate,
                    SpO2 = record.VitalSigns.SpO2,
                    Weight = record.VitalSigns.Weight,
                    Height = record.VitalSigns.Height,
                    RecordedAt = record.VitalSigns.RecordedAt
                } : null
            };
        }
    }
}
