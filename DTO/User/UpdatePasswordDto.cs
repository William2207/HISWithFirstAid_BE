using System.ComponentModel.DataAnnotations;

namespace FirstAidAPI.DTO.User
{
    public class UpdatePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        public string NewPassword { get; set; } = string.Empty;
    }
}
