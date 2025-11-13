using System.ComponentModel.DataAnnotations.Schema;

namespace FirstAidAPI.Models
{
    public class UserTechniqueProgress
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int TechniqueId { get; set; }
        public Technique Technique { get; set; } = null!;
        public bool Status { get; set; } = false;
        public bool? IsVideoWatched { get; set; } = false;
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime LastAccessedAt { get; set; } = DateTime.UtcNow;
    }
}