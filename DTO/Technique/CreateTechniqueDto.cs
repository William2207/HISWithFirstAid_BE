using System.ComponentModel.DataAnnotations;

namespace FirstAidAPI.DTO.Technique
{
    public class CreateTechniqueDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(300)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Difficulty { get; set; } = string.Empty;

        [StringLength(500)]
        public string VideoUrl { get; set; } = string.Empty;

        [StringLength(500)]
        public string ImageUrl { get; set; } = string.Empty;

        [Range(1, 1440)]
        public int Duration { get; set; }

        [StringLength(100)]
        public string Icon { get; set; } = string.Empty;

        [Required]
        public int TechniqueTypeId { get; set; }

        public List<CreateTechniqueStepDto> Steps { get; set; } = new List<CreateTechniqueStepDto>();
    }
}