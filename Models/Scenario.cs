namespace FirstAidAPI.Models
{
    public class Scenario
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string ModelUrl { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public List<ScenarioStep> ScenarioSteps { get; set; } = new List<ScenarioStep>();
    }
}
