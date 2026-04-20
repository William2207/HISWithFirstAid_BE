using System;

namespace FirstAidAPI.DTO.Doctor
{
    public class DoctorProfileDto
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

        // Doctor specific data
        public string? DepartmentName { get; set; }
        public string? PrimarySpecialtyName { get; set; }
        public string? LicenseNumber { get; set; }
        public string? Qualifications { get; set; }
        public int YearsOfExperience { get; set; }
    }
}
