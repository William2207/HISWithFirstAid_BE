using System.ComponentModel.DataAnnotations.Schema;

namespace FirstAidAPI.Models
{
    public class UserScenarioProgress
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int ScenarioId { get; set; }
        public Scenario Scenario { get; set; } = null!;
        public bool Status { get; set; } = false;
        public int HighestScore { get; set; } = 0;
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime LastAccessedAt { get; set; } = DateTime.UtcNow;
    }
}
