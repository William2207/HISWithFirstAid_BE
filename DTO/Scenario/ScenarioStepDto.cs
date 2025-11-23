namespace FirstAidAPI.DTO.Scenario
{
    public class ScenarioStepDto
    {
        public int? Id { get; set; }
        public int Order { get; set; }
        public string StepType { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Question { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public int TimeLimit { get; set; }
        public int MaxScore { get; set; }
        public int? TechniqueId { get; set; }
        public List<StepOptionDto> Options { get; set; } = new();
    }
}