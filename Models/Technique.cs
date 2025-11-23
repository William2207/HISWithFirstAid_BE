using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static FirstAidAPI.Controllers.TechniquesController;

namespace FirstAidAPI.Models
{
    public class Technique
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public string? VideoUrl { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string Icon { get; set; } = string.Empty;
        public int TechniqueTypeId { get; set; } // Foreign key
        public TechniqueType Type { get; set; } = null!;
        public List<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
        public List<TechniqueStep> TechniqueSteps { get; set; } = new List<TechniqueStep>();
    }
}