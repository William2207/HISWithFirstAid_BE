using System.ComponentModel.DataAnnotations.Schema;

namespace FirstAidAPI.Models
{
    public class AnswerOption
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int QuizQuestionId { get; set; }
        public QuizQuestion QuizQuestion { get; set; } = null!;
        public string AnswerText { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }
}
