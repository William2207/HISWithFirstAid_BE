using System.ComponentModel.DataAnnotations;

namespace FirstAidAPI.DTO
{
    public class UpdateTechniqueDto
    {
        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(300)]
        public string? Title { get; set; }

        public string? Description { get; set; }

        [StringLength(50)]
        public string? Difficulty { get; set; }

        [StringLength(500)]
        public string? VideoUrl { get; set; }

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [Range(1, 1440)]
        public int? Duration { get; set; }

        [StringLength(100)]
        public string? Icon { get; set; }

        public int? TechniqueTypeId { get; set; }
    }
}