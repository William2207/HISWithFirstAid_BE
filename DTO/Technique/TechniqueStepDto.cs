namespace FirstAidAPI.DTO.Technique
{
    public class TechniqueStepDto
    {
        public int Id { get; set; }
        public int StepNumber { get; set; }
        public string Instruction { get; set; } = string.Empty;
        public string ExpectedAction { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string? ImageUrl { get; set; } = string.Empty;
    }
}