namespace FirstAidAPI.DTO.PracticalCourse
{
    public class CreatePracticalCourseDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int DurationMinutes { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public bool IsPublished { get; set; }
        public DateOnly CreatedAt { get; set; }
        public int MaxStudents { get; set; }
        public List<string> Highlights { get; set; } = [];
        public List<string> Requirements { get; set; } = [];
    }
}