using System.Text.Json.Serialization;
using static FirstAidAPI.Controllers.TechniquesController;

namespace FirstAidAPI.Models
{
    public class Technique
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string Icon { get; set; } = string.Empty;
        public List<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
        public List<ScenarioTechnique> ScenarioTechniques { get; set; } = new List<ScenarioTechnique>();
        public List<TechniqueStep> TechniqueSteps { get; set; } = new List<TechniqueStep>();
    }
}