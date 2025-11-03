using System.Text.Json.Serialization;

namespace FirstAidAPI.Models
{
    public class ScenarioTechnique
    {
        public int ScenarioId { get; set; }
        public int TechniqueId { get; set; }

        // Optional: Thêm metadata nếu cần
        public int Order { get; set; } = 0;

        // Navigation properties
        [JsonIgnore]
        public Scenario Scenario { get; set; } = null!;

        [JsonIgnore]
        public Technique Technique { get; set; } = null!;
    }
}
