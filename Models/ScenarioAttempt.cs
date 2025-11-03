namespace FirstAidAPI.Models
{
    public class ScenarioAttempt
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int ScenarioId { get; set; }
        public Scenario Scenario { get; set; } = null!;

        public int Score { get; set; }
        public bool IsPassed { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public int TimeSpentSeconds { get; set; }
        public DateTime AttemptedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }

        public List<StepAnswer> StepAnswers { get; set; } = new List<StepAnswer>();
    }
}