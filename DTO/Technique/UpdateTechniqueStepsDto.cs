namespace FirstAidAPI.DTO.Technique
{
    public class UpdateTechniqueStepsDto
    {
        public List<UpdateTechniqueStepDto> Steps { get; set; } = new();
    }
}