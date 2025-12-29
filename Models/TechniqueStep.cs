using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FirstAidAPI.Models
{
    public class TechniqueStep
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? TechniqueId { get; set; }

        [JsonIgnore]
        public Technique? Technique { get; set; }

        public int StepNumber { get; set; }
        public string Instruction { get; set; } = string.Empty;
        public string ExpectedAction { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string? ImageUrl { get; set; } = string.Empty;
    }
}
