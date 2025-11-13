using System.ComponentModel.DataAnnotations;

namespace FirstAidAPI.DTO
{
    public class UpdateAnswerOptionDto
    {
        public int? Id { get; set; } // Nullable để phân biệt giữa update và thêm mới

        [Required]
        public string AnswerText { get; set; } = string.Empty;

        [Required]
        public bool IsCorrect { get; set; }
    }
}