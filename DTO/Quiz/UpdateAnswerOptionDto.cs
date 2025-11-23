using System.ComponentModel.DataAnnotations;

namespace FirstAidAPI.DTO.Quiz
{
    public class UpdateAnswerOptionDto
    {
        public int? Id { get; set; }

        [Required]
        public string AnswerText { get; set; } = string.Empty;

        [Required]
        public bool IsCorrect { get; set; }
    }
}