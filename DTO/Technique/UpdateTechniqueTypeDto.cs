using System.ComponentModel.DataAnnotations;

namespace FirstAidAPI.DTO.Technique
{
    public class UpdateTechniqueTypeDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name must not exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description must not exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Icon must not exceed 50 characters")]
        public string? Icon { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "Color must not exceed 20 characters")]
        public string? Color { get; set; } = string.Empty;
    }
}