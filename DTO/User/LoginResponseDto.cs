namespace FirstAidAPI.DTO.User
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IList<string> Roles { get; set; } = new List<string>();

        // Thông tin đầy đủ của user, tránh phải gọi thêm /users/me
        public UserDto? User { get; set; }
    }
}
