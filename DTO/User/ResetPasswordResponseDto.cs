namespace FirstAidAPI.DTO.User
{
    public class ResetPasswordResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? NewPassword { get; set; }
    }
}
