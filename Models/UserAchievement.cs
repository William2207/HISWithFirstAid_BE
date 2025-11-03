namespace FirstAidAPI.Models
{
    public class UserAchievement
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public string AchievementType { get; set; } = string.Empty; // FirstScenario, Perfect Score, FastLearner, etc.
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public DateTime EarnedAt { get; set; } = DateTime.UtcNow;
    }
}
