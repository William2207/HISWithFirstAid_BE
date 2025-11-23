using System.Text.Json.Serialization;

namespace FirstAidAPI.Models
{
    public class ScenarioStep
    {
        public int Id { get; set; }
        public int ScenarioId { get; set; }
        public int Order { get; set; }
        public string StepType { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Question { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;

        public int TimeLimit { get; set; } = 0;
        public int MaxScore { get; set; } = 10;

        public List<StepOption> Options { get; set; } = new List<StepOption>();

        [JsonIgnore]
        public Scenario Scenario { get; set; } = null!;
    }
}