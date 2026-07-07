using FirstAidAPI.DTO.Patient;
using FirstAidAPI.Models;
using FirstAidAPI.Repository;
using Microsoft.AspNetCore.Identity;

namespace FirstAidAPI.Service.Implement
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly UserManager<User> _userManager;

        public PatientService(IPatientRepository patientRepository, UserManager<User> userManager)
        {
            _patientRepository = patientRepository;
            _userManager = userManager;
        }

        public async Task<PatientProfileDto?> GetPatientProfileAsync(int userId)
        {
            var patient = await _patientRepository.GetByUserIdAsync(userId);
            if (patient == null) return null;

            var user = patient.User;
            if (user == null) return null;

            return new PatientProfileDto
            {
                Id = patient.Id,
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Avatar = user.Avatar,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Address = user.Address,
                IdCard = user.IdCard,
                InsuranceNumber = patient.InsuranceNumber,
                BloodType = patient.BloodType,
                Allergies = patient.Allergies,
                MedicalHistory = patient.MedicalHistory,
                EmergencyContactName = patient.EmergencyContactName,
                EmergencyContact = patient.EmergencyContact,
                EmergencyContactRelationship = patient.EmergencyContactRelationship
            };
        }

        public async Task<List<PatientProfileDto>> GetPatientsBySpecialtyAsync(int specialtyId)
        {
            var patients = await _patientRepository.GetPatientsBySpecialtyAsync(specialtyId);
            return patients.Select(patient =>
            {
                var user = patient.User;
                var latestDiagnosis = patient.MedicalRecords?
                    .Where(mr => mr.DiagnosisName != null)
                    .OrderByDescending(mr => mr.CreatedAt)
                    .FirstOrDefault()?.DiagnosisName ?? "Chưa có chẩn đoán";
                return new PatientProfileDto
                {
                    Id = patient.Id,
                    UserId = user?.Id ?? 0,
                    FullName = user?.FullName ?? patient.FullName,
                    Email = user?.Email ?? patient.Email,
                    PhoneNumber = user?.PhoneNumber ?? patient.PhoneNumber,
                    Avatar = user?.Avatar,
                    DateOfBirth = user?.DateOfBirth ?? patient.DateOfBirth,
                    Gender = user?.Gender ?? patient.Gender,
                    Address = user?.Address ?? patient.Address,
                    IdCard = user?.IdCard ?? patient.IdCard,
                    InsuranceNumber = patient.InsuranceNumber,
                    BloodType = patient.BloodType,
                    Allergies = patient.Allergies,
                    MedicalHistory = patient.MedicalHistory,
                    Diagnosis = latestDiagnosis,
                    EmergencyContactName = patient.EmergencyContactName,
                    EmergencyContact = patient.EmergencyContact,
                    EmergencyContactRelationship = patient.EmergencyContactRelationship
                };
            }).ToList();
        }

        public async Task<bool> UpdatePatientProfileAsync(int userId, UpdatePatientProfileDto updateDto)
        {
            // Update User
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;

            if (updateDto.FullName != null) user.FullName = updateDto.FullName;
            if (updateDto.PhoneNumber != null) user.PhoneNumber = updateDto.PhoneNumber;
            if (updateDto.Avatar != null) user.Avatar = updateDto.Avatar;
            if (updateDto.DateOfBirth.HasValue) user.DateOfBirth = DateTime.SpecifyKind(updateDto.DateOfBirth.Value, DateTimeKind.Utc);
            if (updateDto.Gender != null) user.Gender = updateDto.Gender;
            if (updateDto.Address != null) user.Address = updateDto.Address;
            if (updateDto.IdCard != null) user.IdCard = updateDto.IdCard;

            var userResult = await _userManager.UpdateAsync(user);
            if (!userResult.Succeeded) return false;

            // Update Patient
            var patient = await _patientRepository.GetByUserIdAsync(userId);
            if (patient != null)
            {
                if (updateDto.InsuranceNumber != null) patient.InsuranceNumber = updateDto.InsuranceNumber;
                if (updateDto.BloodType != null) patient.BloodType = updateDto.BloodType;
                if (updateDto.Allergies != null) patient.Allergies = updateDto.Allergies;
                if (updateDto.MedicalHistory != null) patient.MedicalHistory = updateDto.MedicalHistory;
                if (updateDto.EmergencyContactName != null) patient.EmergencyContactName = updateDto.EmergencyContactName;
                if (updateDto.EmergencyContact != null) patient.EmergencyContact = updateDto.EmergencyContact;
                if (updateDto.EmergencyContactRelationship != null) patient.EmergencyContactRelationship = updateDto.EmergencyContactRelationship;

                await _patientRepository.UpdateAsync(patient);
            }

            return true;
        }
    }
}
