using System;

namespace FirstAidAPI.DTO.Patient
{
    public class PatientProfileDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        // User data
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Avatar { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? IdCard { get; set; }

        // Patient specific data
        public string? InsuranceNumber { get; set; }
        public string? BloodType { get; set; }
        public string? Allergies { get; set; }
        public string? MedicalHistory { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContact { get; set; }
        public string? EmergencyContactRelationship { get; set; }
    }
}
