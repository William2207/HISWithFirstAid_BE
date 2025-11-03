namespace FirstAidAPI.DTO
{
    public class UserScenarioProgressDto
    {
        /// <summary>
        /// ID của kịch bản (dùng để điều hướng).
        /// </summary>
        public int ScenarioId { get; set; }

        /// <summary>
        /// Tên của kịch bản (lấy từ Scenario.Name).
        /// </summary>
        public string ScenarioName { get; set; } = string.Empty;

        /// <summary>
        /// Trạng thái hoàn thành (map từ thuộc tính Status).
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Điểm số cao nhất người dùng đạt được trong kịch bản này.
        /// </summary>
        public int HighestScore { get; set; }

        /// <summary>
        /// Lần cuối cùng truy cập kịch bản này.
        /// </summary>
        public DateTime LastAccessedAt { get; set; }
    }
}