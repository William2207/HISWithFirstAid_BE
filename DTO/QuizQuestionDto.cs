namespace FirstAidAPI.DTO
{
    public class QuizQuestionDto
    {
        public int Id { get; set; }
        public int TechniqueId { get; set; }
        public string TechniqueName { get; set; } = string.Empty;
        public string QuestionText { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public List<AnswerOptionDto>? AnswerOptions { get; set; }
    }
}