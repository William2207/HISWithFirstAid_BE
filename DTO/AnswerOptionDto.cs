namespace FirstAidAPI.DTO
{
    public class AnswerOptionDto
    {
        public int? Id { get; set; }
        public string AnswerText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }
}