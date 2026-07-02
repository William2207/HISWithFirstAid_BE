using FirstAidAPI.DTO.User;
using FirstAidAPI.Models;
using FirstAidAPI.Service;
using FirstAidAPI.Service.Implement;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace FirstAidAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;
    private readonly IAccountService _accountService;
    private readonly IPasswordResetService _passwordResetService;

    public AccountController(
        UserManager<User> userManager,
        ITokenService tokenService,
        IEmailService emailService,
        IPasswordResetService passwordResetService,
        IAccountService accountService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _emailService = emailService;
        _passwordResetService = passwordResetService;
        _accountService = accountService;
    }

    // POST: api/account/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var message = await _accountService.RegisterAsync(registerDto, Request.Scheme);
            return Ok(new { Message = message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    // POST: api/account/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _accountService.LoginAsync(loginDto);
            return Ok(new
            {
                Message = "Đăng nhập thành công!",
                Token = result.Token,
                User = result.User   // UserDto đầy đủ, frontend không cần gọi thêm /users/me
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { Message = ex.Message });
        }
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenApiModel tokenApiModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var newTokens = await _accountService.RefreshTokenAsync(tokenApiModel);
            return Ok(newTokens);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { Message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { Message = "Internal server error" });
        }
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(int userId, string token)
    {
        if (userId == default || string.IsNullOrEmpty(token))
        {
            return BadRequest("Dữ liệu không hợp lệ.");
        }

        try
        {
            await _accountService.ConfirmEmailAsync(userId, token);
            return Ok("Xác thực email thành công! Bây giờ bạn có thể đăng nhập.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // POST: api/account/forgot-password
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _passwordResetService.SendPasswordResetOtpAsync(forgotPasswordDto.Email);
            if (!result)
            {
                return BadRequest(new { Message = "Không thể gửi email. Vui lòng thử lại sau." });
            }

            return Ok(new { Message = "OTP đã được gửi đến email của bạn. Vui lòng kiểm tra email." });
        }
        catch (Exception)
        {
            return StatusCode(500, new { Message = "Đã xảy ra lỗi khi gửi email." });
        }
    }

    // POST: api/account/verify-otp
    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto verifyOtpDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var newPassword = await _passwordResetService.VerifyOtpAndResetPasswordAsync(verifyOtpDto.Email, verifyOtpDto.Otp);

            return Ok(new ResetPasswordResponseDto
            {
                Success = true,
                Message = "Mật khẩu đã được đặt lại thành công. Mật khẩu mới đã được gửi đến email của bạn.",
                NewPassword = newPassword
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { Message = "Đã xảy ra lỗi khi đặt lại mật khẩu." });
        }
    }

    // POST: api/account/create-account-admin
    [HttpPost("create-account-admin")]
    public async Task<IActionResult> CreateAccountAsAdmin([FromBody] AdminCreateAccountDto adminCreateAccountDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var message = await _accountService.CreateAccountAsAdminAsync(adminCreateAccountDto);
            return Ok(new { Message = message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = $"Lỗi khi tạo tài khoản: {ex.Message}" });
        }
    }
}
