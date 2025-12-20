namespace FirstAidAPI.DTO.PracticalCourse
{
    public class UpdatePracticalCourseDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public string? Location { get; set; }
        public decimal? Price { get; set; }
        public int? DurationMinutes { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public bool? IsPublished { get; set; }
        public int? MaxStudents { get; set; }
        public List<string>? Highlights { get; set; }
        public List<string>? Requirements { get; set; }
    }
}