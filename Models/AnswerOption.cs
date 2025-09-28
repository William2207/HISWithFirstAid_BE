namespace FirstAidAPI.Models
{
    public class AnswerOption
    {
        public int Id { get; set; }
        public int QuizQuestionId { get; set; }
        public string AnswerText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }
}
