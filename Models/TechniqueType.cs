using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FirstAidAPI.Models
{
    public class TechniqueType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty; // "Relaxation", "Cognitive", etc.
        public string Description { get; set; } = string.Empty;
        public string? Icon { get; set; } = string.Empty;
        public string? Color { get; set; } = string.Empty; // Để UI

        [JsonIgnore]
        public List<Technique> Techniques { get; set; } = new List<Technique>();
    }
}
