namespace FirstAidAPI.DTO
{
    public class UserTechniqueProgressDto
    {
        /// <summary>
        /// ID của kỹ thuật (dùng để điều hướng).
        /// </summary>
        public int TechniqueId { get; set; }

        /// <summary>
        /// Tên của kỹ thuật (lấy từ Technique.Name).
        /// </summary>
        public string TechniqueName { get; set; } = string.Empty;

        /// <summary>
        /// Trạng thái hoàn thành (map từ thuộc tính Status).
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Lần cuối cùng truy cập kỹ thuật này.
        /// </summary>
        public DateTime LastAccessedAt { get; set; }
    }
}