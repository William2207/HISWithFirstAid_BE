namespace FirstAidAPI.DTO
{
    public class ScenarioTechniqueDto
    {
        public int TechniqueId { get; set; }
        public int Order { get; set; }
        public TechniqueDto? Technique { get; set; } = null;
    }
}