namespace FirstAidAPI.DTO.Technique
{
    public class TechniqueDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public string? VideoUrl { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string Icon { get; set; } = string.Empty;
        public int TechniqueTypeId { get; set; }
        public string? TechniqueTypeName { get; set; }
    }
}