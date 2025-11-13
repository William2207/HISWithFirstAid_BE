using System.ComponentModel.DataAnnotations;

namespace FirstAidAPI.DTO
{
    public class CreateTechniqueStepDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Step number must be greater than 0")]
        public int StepNumber { get; set; }

        [Required(ErrorMessage = "Instruction is required")]
        [StringLength(1000, ErrorMessage = "Instruction cannot exceed 1000 characters")]
        public string Instruction { get; set; } = string.Empty;

        [Required(ErrorMessage = "Expected action is required")]
        [StringLength(1000, ErrorMessage = "Expected action cannot exceed 1000 characters")]
        public string ExpectedAction { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Duration must be non-negative")]
        public int Duration { get; set; } = 0;

        [StringLength(500, ErrorMessage = "Image URL cannot exceed 500 characters")]
        [Url(ErrorMessage = "Image URL must be a valid URL")]
        public string? ImageUrl { get; set; } = string.Empty;
    }
}