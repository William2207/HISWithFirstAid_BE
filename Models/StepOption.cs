using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FirstAidAPI.Models
{
    public class StepOption
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int StepId { get; set; }

        public string OptionKey { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;

        // ⚠️ Không gửi xuống client
        public bool IsCorrect { get; set; }

        public int ScoreValue { get; set; } = 0;

        public string FeedbackCorrect { get; set; } = string.Empty;
        public string FeedbackIncorrect { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;

        public int? NextStepId { get; set; }
        public bool EndScenario { get; set; } = false;

        [JsonIgnore]
        public ScenarioStep Step { get; set; } = null!;
    }
}