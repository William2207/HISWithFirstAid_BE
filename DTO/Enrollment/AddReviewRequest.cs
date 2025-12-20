namespace FirstAidAPI.DTO.Enrollment
{
    public class AddReviewRequest
    {
        public int EnrollmentId { get; set; }
        public int Rating { get; set; }
        public string? Review { get; set; }
    }
}