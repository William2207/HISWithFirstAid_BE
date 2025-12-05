namespace FirstAidAPI.DTO.Enrollment
{
    public class StudentDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public DateTime EnrolledAt { get; set; }
    }
}