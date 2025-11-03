namespace FirstAidAPI.DTO
{
    public class ScenarioAttemptDto
    {
        /// <summary>
        /// ID của lần thử.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tên của kịch bản (lấy từ Scenario.Name).
        /// </summary>
        public string ScenarioName { get; set; } = string.Empty;

        /// <summary>
        /// Điểm số đạt được.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Kết quả đậu hay rớt.
        /// </summary>
        public bool IsPassed { get; set; }

        /// <summary>
        /// Thời điểm thực hiện lần thử.
        /// </summary>
        public DateTime AttemptedAt { get; set; }
    }
}