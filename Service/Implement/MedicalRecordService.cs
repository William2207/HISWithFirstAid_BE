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

        public MedicalRecordService(
            IMedicalRecordRepository medicalRecordRepository,
            IAppointmentRepository appointmentRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<MedicalRecordDTO> CreateMedicalRecordAsync(int doctorId, CreateMedicalRecordRequest request)
        {
            // Kiểm tra xem bệnh án đã tồn tại cho lịch hẹn này chưa
            if (await _medicalRecordRepository.ExistsByAppointmentIdAsync(request.AppointmentId))
            {
                throw new BusinessException("Lịch hẹn này đã có hồ sơ bệnh án.");
            }

            // (Tuỳ chọn) - Có thể kiểm tra Appointment có thuộc về Doctor này không ở đây
            // var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId);
            // if (appointment == null) throw new NotFoundException("Không tìm thấy lịch hẹn");
            // if (appointment.DoctorId != doctorId) throw new UnauthorizedException("Bác sĩ không có quyền tạo bệnh án cho lịch hẹn này");
            
            var medicalRecord = new MedicalRecord
            {
                AppointmentId = request.AppointmentId,
                DoctorId = doctorId,
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
                CreatedAt = DateTime.UtcNow
            };

            if (request.VitalSigns != null && request.VitalSigns.Any())
            {
                foreach (var vital in request.VitalSigns)
                {
                    medicalRecord.VitalSigns.Add(new VitalSign
                    {
                        NurseId = vital.NurseId,
                        BloodPressure = vital.BloodPressure,
                        HeartRate = vital.HeartRate,
                        Temperature = vital.Temperature,
                        RespiratoryRate = vital.RespiratoryRate,
                        SpO2 = vital.SpO2,
                        Weight = vital.Weight,
                        Height = vital.Height,
                        RecordedAt = DateTime.UtcNow
                    });
                }
            }

            var createdRecord = await _medicalRecordRepository.CreateAsync(medicalRecord);
            return await GetMedicalRecordByIdAsync(createdRecord.Id); // Reload để có Doctor info
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
            record.NextAppointmentDate = request.NextAppointmentDate ?? record.NextAppointmentDate;
            record.GeneralNotes = request.GeneralNotes ?? record.GeneralNotes;

            if (request.VitalSigns != null && request.VitalSigns.Any())
            {
                foreach (var vital in request.VitalSigns)
                {
                    record.VitalSigns.Add(new VitalSign
                    {
                        NurseId = vital.NurseId,
                        BloodPressure = vital.BloodPressure,
                        HeartRate = vital.HeartRate,
                        Temperature = vital.Temperature,
                        RespiratoryRate = vital.RespiratoryRate,
                        SpO2 = vital.SpO2,
                        Weight = vital.Weight,
                        Height = vital.Height,
                        RecordedAt = DateTime.UtcNow
                    });
                }
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
                CreatedAt = record.CreatedAt,
                VitalSigns = record.VitalSigns.Select(v => new VitalSignDTO
                {
                    Id = v.Id,
                    BloodPressure = v.BloodPressure,
                    HeartRate = v.HeartRate,
                    Temperature = v.Temperature,
                    RespiratoryRate = v.RespiratoryRate,
                    SpO2 = v.SpO2,
                    Weight = v.Weight,
                    Height = v.Height,
                    RecordedAt = v.RecordedAt
                }).ToList()
            };
        }
    }
}
