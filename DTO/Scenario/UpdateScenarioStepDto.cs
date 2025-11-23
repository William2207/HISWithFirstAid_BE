using System.ComponentModel.DataAnnotations;

namespace FirstAidAPI.DTO.Scenario
{
    public class UpdateScenarioStepDto
    {
        [Required]
        public int Id { get; set; }

        // Các trường có thể cập nhật
        public int? Order { get; set; }

        [StringLength(200)]
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Question { get; set; }

        public string? ImageUrl { get; set; }

        public string? VideoUrl { get; set; }

        public int? TimeLimit { get; set; }               // giây, 0 = không giới hạn

        public int? MaxScore { get; set; }

        public string? StepType { get; set; }

        // Danh sách các option (có thể cập nhật toàn bộ hoặc từng phần)
        public List<UpdateStepOptionDto>? Options { get; set; }
    }
}