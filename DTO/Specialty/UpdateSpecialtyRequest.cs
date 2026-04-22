using System.ComponentModel.DataAnnotations;

namespace FirstAidAPI.DTO.Specialty
{
    public class UpdateSpecialtyRequest
    {
        [Required(ErrorMessage = "Tên chuyên khoa là bắt buộc.")]
        [MaxLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự.")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự.")]
        public string? Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá phải là số không âm.")]
        public decimal Price { get; set; }

        public bool IsActive { get; set; }
    }
}
