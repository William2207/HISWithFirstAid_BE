using System.ComponentModel.DataAnnotations;

namespace FirstAidAPI.DTO
{
    public class CreateScenarioStepDto
    {
        [Required]
        public string StepType { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Question { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public int TimeLimit { get; set; } = 0;
        public int MaxScore { get; set; } = 10;
        public int? TechniqueId { get; set; }

        // Danh sách options cho step
        public List<CreateStepOptionDto> Options { get; set; } = new List<CreateStepOptionDto>();
    }
}