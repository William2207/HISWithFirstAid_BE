using System.Text.Json.Serialization;
using static FirstAidAPI.Controllers.TechniquesController;

namespace FirstAidAPI.Models
{
    public class Technique
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Difficulty { get; set; }
        public string VideoUrl { get; set; }
        public string ImageUrl { get; set; }
        public int Duration { get; set; }
        public string Icon { get; set; }
        public int? ScenarioId { get; set; } // Khóa ngoại liên kết với Scenario
        [JsonIgnore]
        public Scenario? Scenario { get; set; } // Thuộc tính điều hướng
        public List<TechniqueStep> TechniqueSteps { get; set; } = new List<TechniqueStep>();
    }
}
