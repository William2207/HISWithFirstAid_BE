namespace FirstAidAPI.DTO.Scenario
{
    public class StepOptionDto
    {
        public int? Id { get; set; }
        public string OptionKey { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public string FeedbackCorrect { get; set; } = string.Empty;
        public int ScoreValue { get; set; } = 0;
        public string FeedbackIncorrect { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}