using FirstAidAPI.DTO.User;
using FirstAidAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace FirstAidAPI.Service.Implement
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly IUrlHelper _urlHelper;
        private readonly ITokenService _tokenService;

        public AccountService(
            UserManager<User> userManager,
            IEmailService emailService,
            IUrlHelper urlHelper,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _emailService = emailService;
            _urlHelper = urlHelper;
            _tokenService = tokenService;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Email chưa được xác thực hoặc tài khoản không tồn tại");
            }

            if (!user.EmailConfirmed)
            {
                throw new UnauthorizedAccessException("Email chưa được xác thực hoặc tài khoản không tồn tại");
            }

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                throw new UnauthorizedAccessException("Email hoặc mật khẩu không hợp lệ.");
            }

            // Cập nhật thời gian đăng nhập
            user.LastLoginAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            // Lấy roles
            var roles = await _userManager.GetRolesAsync(user);

            // Tạo token
            var token = _tokenService.CreateToken(user, roles);

            return new LoginResponseDto
            {
                Token = token,
                Email = user.Email!,
                Roles = roles
            };
        }

        public async Task<string> RegisterAsync(RegisterDto registerDto, string scheme)
        {
            var user = new User
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                DateOfBirth = DateTime.SpecifyKind(registerDto.DateOfBirth, DateTimeKind.Utc),
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            await _userManager.AddToRoleAsync(user, "User");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationLink = _urlHelper.Action(
                "ConfirmEmail",
                "Account",
                new { userId = user.Id, token = token },
                scheme);

            if (string.IsNullOrEmpty(confirmationLink))
            {
                throw new InvalidOperationException("Không thể tạo link xác thực.");
            }

            var emailBody = $"Chào {user.FullName},<br><br>" +
                           $"Cảm ơn bạn đã đăng ký. Vui lòng nhấp vào liên kết sau để xác thực tài khoản của bạn:<br>" +
                           $"<a href='{HtmlEncoder.Default.Encode(confirmationLink)}'>XÁC THỰC TÀI KHOẢN</a>";

            await _emailService.SendEmailAsync(user.Email, "Xác thực tài khoản của bạn", emailBody);

            return "Đăng ký thành công. Vui lòng kiểm tra email của bạn để xác thực tài khoản.";
        }

        public async Task ConfirmEmailAsync(int userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new KeyNotFoundException("Không tìm thấy người dùng.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Xác thực email thất bại.");
            }
        }
    }
}
