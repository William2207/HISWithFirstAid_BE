using System.ComponentModel.DataAnnotations;

namespace FirstAidAPI.DTO.Scenario
{
    public class UpdateScenarioDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        public string Type { get; set; } = string.Empty;

        [Required]
        public string Difficulty { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int Duration { get; set; }

        public string Icon { get; set; } = string.Empty;

        [Range(0, 100)]
        public int PassingScore { get; set; } = 70;

        public bool IsPublished { get; set; } = true;

        public List<UpdateScenarioStepDto> ScenarioSteps { get; set; } = new();
    }
}