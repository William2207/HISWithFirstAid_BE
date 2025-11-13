namespace FirstAidAPI.DTO
{
    public class TechniqueTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public List<TechniqueDto> Techniques { get; set; } = new List<TechniqueDto>();
    }
}