namespace FirstAidAPI.DTO.Enrollment
{
    public class EnrollmentDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public DateOnly CourseStartDate { get; set; }
        public DateOnly CourseEndDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public DateTime EnrolledAt { get; set; }
        public int? Rating { get; set; }
    }
}