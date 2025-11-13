using System.ComponentModel.DataAnnotations.Schema;

namespace FirstAidAPI.Models
{
    public class QuizQuestion
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int TechniqueId { get; set; }
        public Technique Technique { get; set; } = null!;
        public string QuestionText { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public List<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();
    }
}