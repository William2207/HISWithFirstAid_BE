using System.ComponentModel.DataAnnotations;

namespace FirstAidAPI.DTO
{
    public class CreateScenarioTechniqueDto
    {
        [Required]
        public int TechniqueId { get; set; }

        public int Order { get; set; } = 0;
    }
}