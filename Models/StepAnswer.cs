namespace FirstAidAPI.Models
{
    public class StepAnswer
    {
        public int Id { get; set; }
        public int ScenarioAttemptId { get; set; }
        public ScenarioAttempt ScenarioAttempt { get; set; } = null!;
        public int StepId { get; set; }
        public ScenarioStep Step { get; set; } = null!;

        public string UserAnswer { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int PointsEarned { get; set; }
        public int TimeSpentSeconds { get; set; }
        public DateTime AnsweredAt { get; set; } = DateTime.UtcNow;
    }
}
