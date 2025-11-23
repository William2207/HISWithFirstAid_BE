using System.ComponentModel.DataAnnotations;

namespace FirstAidAPI.DTO.Scenario
{
    public class UpdateStepOptionDto
    {
        public int? Id { get; set; }

        [Required]
        public string OptionKey { get; set; } = string.Empty;

        [Required]
        public string Text { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public bool? IsCorrect { get; set; }

        public int? ScoreValue { get; set; }

        public string? FeedbackCorrect { get; set; }

        public string? FeedbackIncorrect { get; set; }

        public string? Explanation { get; set; }

        public int? NextStepId { get; set; }

        public bool? EndScenario { get; set; }

        // Dùng để xóa option (nếu API hỗ trợ soft-delete hoặc explicit delete flag)
        public bool IsDeleted { get; set; } = false;
    }
}