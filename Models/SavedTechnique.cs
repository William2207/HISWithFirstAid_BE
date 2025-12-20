namespace FirstAidAPI.Models
{
    public class SavedTechnique
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int TechniqueId { get; set; }
        public Technique Technique { get; set; } = null!;

        public string? Notes { get; set; } // Ghi chú cá nhân
        public DateTime SavedAt { get; set; }
        public int Priority { get; set; } = 0; // 0: Normal, 1: Important, 2: Urgent
    }
}