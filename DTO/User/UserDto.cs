namespace FirstAidAPI.DTO.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Avatar { get; set; }
        public DateTime DateOfBirth { get; set; }
        public IList<string>? Role { get; set; }
        public int? SpecialtyId { get; set; }
        public string? SpecialtyName { get; set; }
        public int? DoctorId { get; set; }
        public int? NurseId { get; set; }
        public bool IsHeadDoctor { get; set; }
        public bool IsHeadNurse { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }
}