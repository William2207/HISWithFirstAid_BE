namespace FirstAidAPI.DTO.SavedTechniques
{
    public class SavedTechniqueNewDto
    {
        public int Id { get; set; }
        public int TechniqueId { get; set; }
        public string TechniqueName { get; set; } = string.Empty;
        public string TechniqueTitle { get; set; } = string.Empty;
        public string TechniqueDifficulty { get; set; } = string.Empty;
        public DateTime SavedAt { get; set; }
    }
}