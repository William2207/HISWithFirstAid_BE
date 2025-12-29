using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FirstAidAPI.Models
{
    public class Scenario
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string? Icon { get; set; } = string.Empty;
        public List<ScenarioStep> ScenarioSteps { get; set; } = new List<ScenarioStep>();
        public int PassingScore { get; set; } = 70;
        public bool IsPublished { get; set; } = true;
    }
}
