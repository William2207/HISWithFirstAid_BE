namespace FirstAidAPI.DTO
{
    public class ScenarioDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string Icon { get; set; } = string.Empty;
        public int PassingScore { get; set; }
        public bool IsPublished { get; set; }
        public int StepCount { get; set; }
    }

    public class ScenarioDetailDto : ScenarioDto
    {
        public List<ScenarioTechniqueDto> ScenarioTechniques { get; set; } = new();
        public List<ScenarioStepDto> ScenarioSteps { get; set; } = new();
    }
}