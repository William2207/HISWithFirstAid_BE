using System.ComponentModel.DataAnnotations;

namespace FirstAidAPI.DTO
{
    public class CreateAnswerOptionDto
    {
        [Required]
        public string AnswerText { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }
    }
}