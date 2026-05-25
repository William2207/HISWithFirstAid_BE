using FirstAidAPI.DTO.Admission;
using FirstAidAPI.Models;
using FirstAidAPI.Repository;

namespace FirstAidAPI.Service.Implement
{
    public class AdmissionService : IAdmissionService
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IBedRepository _bedRepository;
        private readonly IAdmissionRepository _admissionRepository;
        private readonly INurseRepository _nurseRepository;

        public AdmissionService(
            IMedicalRecordRepository medicalRecordRepository,
            IBedRepository bedRepository,
            IAdmissionRepository admissionRepository,
            INurseRepository nurseRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;
            _bedRepository = bedRepository;
            _admissionRepository = admissionRepository;
            _nurseRepository = nurseRepository;
        }

        public async Task<List<PendingAdmissionDto>> GetPendingAdmissionsAsync()
        {
            var records = await _medicalRecordRepository.GetPendingAdmissionsAsync();

            return records.Select(m => new PendingAdmissionDto
            {
                MedicalRecordId = m.Id,
                PatientId = m.PatientId,
                PatientName = m.Patient.FullNameDisplay,
                DiagnosisName = m.DiagnosisName,
                DoctorName = m.Doctor?.User?.FullName,
                CreatedAt = m.CreatedAt
            }).ToList();
        }

        public async Task<List<BedDto>> GetAvailableBedsAsync()
        {
            var beds = await _bedRepository.GetAvailableBedsAsync();

            return beds.Select(b => new BedDto
            {
                Id = b.Id,
                BedNumber = b.BedNumber,
                Status = b.Status,
                WardId = b.WardId,
                RoomNumber = b.Ward.RoomNumber,
                WardType = b.Ward.WardType,
                Floor = b.Ward.Floor
            }).ToList();
        }

        public async Task<AdmissionRecordDto> AssignBedAsync(int nurseUserId, AssignBedRequest request)
        {
            var nurse = await _nurseRepository.GetByUserIdAsync(nurseUserId)
                ?? throw new InvalidOperationException("Nurse profile not found for the current user.");

            var bed = await _bedRepository.GetByIdAsync(request.BedId)
                ?? throw new InvalidOperationException($"Bed {request.BedId} not found.");

            if (bed.Status != "AVAILABLE")
                throw new InvalidOperationException($"Bed {bed.BedNumber} is not available (current status: {bed.Status}).");

            // Cập nhật trạng thái giường
            bed.Status = "OCCUPIED";
            bed.CurrentPatientId = request.PatientId;
            await _bedRepository.UpdateAsync(bed);

            // Tạo bản ghi nhập viện
            var admissionRecord = new AdmissionRecord
            {
                PatientId = request.PatientId,
                BedId = request.BedId,
                MedicalRecordId = request.MedicalRecordId,
                AdmittedByNurseId = nurse.Id,
                Notes = request.Notes,
                AdmittedAt = DateTime.UtcNow
            };

            var savedRecord = await _admissionRepository.CreateAsync(admissionRecord);

            return new AdmissionRecordDto
            {
                Id = savedRecord.Id,
                PatientId = savedRecord.PatientId,
                BedId = savedRecord.BedId,
                BedNumber = bed.BedNumber,
                RoomNumber = bed.Ward.RoomNumber,
                WardType = bed.Ward.WardType,
                MedicalRecordId = savedRecord.MedicalRecordId,
                NurseName = nurse.User?.FullName,
                AdmittedAt = savedRecord.AdmittedAt,
                Notes = savedRecord.Notes,
                DiagnosisName = null, // Can be loaded if needed, but usually not strictly required right after assign
                DoctorName = null,
                PatientAge = 0,
                PatientGender = "",
                DateOfBirth = null,
                Address = null,
                IdCard = null
            };
        }

        public async Task DischargePatientAsync(int patientId)
        {
            var activeAdmission = await _admissionRepository.GetActiveAdmissionByPatientIdAsync(patientId)
                ?? throw new InvalidOperationException($"No active admission found for patient {patientId}.");

            // Xuất viện: cập nhật AdmissionRecord
            activeAdmission.DischargedAt = DateTime.UtcNow;
            await _admissionRepository.UpdateAsync(activeAdmission);

            // Giải phóng giường
            var bed = await _bedRepository.GetByIdAsync(activeAdmission.BedId)
                ?? throw new InvalidOperationException($"Bed {activeAdmission.BedId} not found during discharge.");

            bed.Status = "AVAILABLE";
            bed.CurrentPatientId = null;
            await _bedRepository.UpdateAsync(bed);
        }

        public async Task<List<AdmissionRecordDto>> GetActiveAdmissionsAsync()
        {
            var records = await _admissionRepository.GetActiveAdmissionsAsync();

            return records.Select(a => new AdmissionRecordDto
            {
                Id = a.Id,
                PatientId = a.PatientId,
                PatientName = a.Patient.FullNameDisplay,
                BedId = a.BedId,
                BedNumber = a.Bed.BedNumber,
                RoomNumber = a.Bed.Ward.RoomNumber,
                WardType = a.Bed.Ward.WardType,
                MedicalRecordId = a.MedicalRecordId,
                NurseName = a.AdmittedByNurse?.User?.FullName,
                AdmittedAt = a.AdmittedAt,
                DischargedAt = a.DischargedAt,
                Notes = a.Notes,
                DiagnosisName = a.MedicalRecord?.DiagnosisName,
                DoctorName = a.MedicalRecord?.Doctor?.User?.FullName,
                PatientAge = a.Patient.DateOfBirthDisplay.HasValue ? DateTime.Now.Year - a.Patient.DateOfBirthDisplay.Value.Year : 0,
                PatientGender = a.Patient.GenderDisplay,
                DateOfBirth = a.Patient.DateOfBirthDisplay,
                Address = a.Patient.AddressDisplay,
                IdCard = a.Patient.IdCardDisplay,
                TreatmentPlan = a.MedicalRecord?.TreatmentPlan,
                Prescription = a.MedicalRecord?.Prescription,
                ChiefComplaint = a.MedicalRecord?.ChiefComplaint
            }).ToList();
        }
        public async Task<List<AdmissionRecordDto>> GetAdmissionHistoryByPatientIdAsync(int patientId)
        {
            var records = await _admissionRepository.GetAllByPatientIdAsync(patientId);

            return records.Select(a => new AdmissionRecordDto
            {
                Id = a.Id,
                PatientId = a.PatientId,
                PatientName = a.Patient?.FullNameDisplay ?? string.Empty,
                BedId = a.BedId,
                BedNumber = a.Bed?.BedNumber ?? string.Empty,
                RoomNumber = a.Bed?.Ward?.RoomNumber ?? string.Empty,
                WardType = a.Bed?.Ward?.WardType ?? string.Empty,
                MedicalRecordId = a.MedicalRecordId,
                DiagnosisName = a.MedicalRecord?.DiagnosisName,
                DoctorName = a.MedicalRecord?.Doctor?.User?.FullName,
                NurseName = a.AdmittedByNurse?.User?.FullName,
                AdmittedAt = a.AdmittedAt,
                DischargedAt = a.DischargedAt,
                Notes = a.Notes,
                PatientAge = 0,
                PatientGender = string.Empty,
                DateOfBirth = null,
                Address = null,
                IdCard = null,
                TreatmentPlan = a.MedicalRecord?.TreatmentPlan,
                Prescription = a.MedicalRecord?.Prescription,
                ChiefComplaint = a.MedicalRecord?.ChiefComplaint
            }).ToList();
        }
    }
}
