using System.ComponentModel.DataAnnotations;

namespace FirstAidAPI.DTO.User
{
    public class UpdateStaffRequest
    {
        [Required(ErrorMessage = "Họ và tên là bắt buộc.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public string? Password { get; set; } // Nếu cung cấp thì đổi mật khẩu

        public int? SpecialtyId { get; set; } // Cho Doctor/Nurse

        public bool IsHead { get; set; } // Trưởng khoa/Điều dưỡng trưởng
    }
}
