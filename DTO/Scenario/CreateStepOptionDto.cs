using System.ComponentModel.DataAnnotations;

namespace FirstAidAPI.DTO.Scenario
{
    public class CreateStepOptionDto
    {
        [Required]
        public string OptionKey { get; set; } = string.Empty;

        [Required]
        public string Text { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int ScoreValue { get; set; } = 0;
        public string FeedbackCorrect { get; set; } = string.Empty;
        public string FeedbackIncorrect { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;
        public int? NextStepId { get; set; }
        public bool EndScenario { get; set; } = false;
    }
}