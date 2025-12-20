using System.ComponentModel.DataAnnotations;

namespace FirstAidAPI.DTO.Quiz
{
    public class UpdateQuizQuestionDto
    {
        [Required]
        public int TechniqueId { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 10)]
        public string QuestionText { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(Easy|Medium|Hard)$")]
        public string Difficulty { get; set; } = string.Empty;

        public List<UpdateAnswerOptionDto>? AnswerOptions { get; set; }
    }
}