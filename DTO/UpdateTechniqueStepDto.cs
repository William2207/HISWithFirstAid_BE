using System.ComponentModel.DataAnnotations;

namespace FirstAidAPI.DTO
{
    public class UpdateTechniqueStepDto
    {
        [Required]
        public int Id { get; set; } // ID của step cần update

        [Range(1, int.MaxValue)]
        public int? StepNumber { get; set; }

        [StringLength(1000)]
        public string? Instruction { get; set; }

        [StringLength(1000)]
        public string? ExpectedAction { get; set; }

        [Range(0, int.MaxValue)]
        public int? Duration { get; set; }

        [StringLength(500)]
        [Url]
        public string? ImageUrl { get; set; }
    }
}