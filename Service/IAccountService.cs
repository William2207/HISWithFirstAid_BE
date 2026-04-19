using FirstAidAPI.DTO.User;

namespace FirstAidAPI.Service
{
    public interface IAccountService
    {
        Task<string> RegisterAsync(RegisterDto registerDto, string scheme);

        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);

        Task ConfirmEmailAsync(int userId, string token);

        Task<string> CreateAccountAsAdminAsync(AdminCreateAccountDto adminCreateAccountDto);
    }
}
