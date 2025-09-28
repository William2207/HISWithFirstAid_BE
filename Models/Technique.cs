using static FirstAidAPI.Controllers.TechniquesController;

namespace FirstAidAPI.Models
{
    public class Technique
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Difficulty { get; set; }
        public int Duration { get; set; }
        public string ModelUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public List<ScenarioStep> ScenarioSteps { get; set; }
    }
}
