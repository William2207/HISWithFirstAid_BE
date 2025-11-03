namespace FirstAidAPI.DTO
{
    public class SavedTechniqueDto
    {
        /// <summary>
        /// ID của bản ghi SavedTechnique (dùng để xóa).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tên của kỹ thuật (lấy từ Technique.Name).
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Loại kỹ thuật (ví dụ: Chấn thương, Tim mạch, lấy từ Technique.Type).
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Độ khó của kỹ thuật (ví dụ: Dễ, Trung bình, Khó, lấy từ Technique.Difficulty).
        /// </summary>
        public string Difficulty { get; set; } = string.Empty;
    }
}