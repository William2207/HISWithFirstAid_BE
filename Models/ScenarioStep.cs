namespace FirstAidAPI.Models
{
    public class ScenarioStep
    {
        public int Id { get; set; }
        public int? TechniqueId { get; set; }
        public int? ScenarioId { get; set; }
        public int StepNumber { get; set; }
        public string Instruction { get; set; } = string.Empty;
        public string ExpectedAction { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public Technique? Technique { get; set; }
        public Scenario? Scenario { get; set; }   
    }
}
