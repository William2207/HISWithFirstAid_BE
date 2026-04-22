using System.ComponentModel.DataAnnotations;

namespace FirstAidAPI.DTO.User
{
    public class AdminCreateAccountDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public int SpecialtyId { get; set; } // For Nurse, Doctor
    }
}
