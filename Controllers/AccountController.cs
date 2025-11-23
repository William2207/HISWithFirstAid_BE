using FirstAidAPI.DTO.User;
using FirstAidAPI.Models;
using FirstAidAPI.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;

    public AccountController(UserManager<User> userManager, ITokenService tokenService, IEmailService emailService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _emailService = emailService;
    }

    // POST: api/account/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = new User
        {
            UserName = registerDto.Email, // Identity yêu cầu UserName
            Email = registerDto.Email,
            FullName = registerDto.FullName,
            DateOfBirth = registerDto.DateOfBirth.Date,
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        // Tạo token xác thực email
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        // Tạo link xác thực
        var confirmationLink = Url.Action(
            nameof(ConfirmEmail),
            "Account",
            new { userId = user.Id, token = token },
            Request.Scheme);

        if (confirmationLink == null)
        {
            return StatusCode(500, "Không thể tạo link xác thực.");
        }

        // Gửi email
        var emailBody = $"Chào {user.FullName},<br><br>Cảm ơn bạn đã đăng ký. Vui lòng nhấp vào liên kết sau để xác thực tài khoản của bạn:<br><a href='{HtmlEncoder.Default.Encode(confirmationLink)}'>XÁC THỰC TÀI KHOẢN</a>";
        await _emailService.SendEmailAsync(user.Email, "Xác thực tài khoản của bạn", emailBody);

        return Ok(new { Message = "Đăng ký thành công. Vui lòng kiểm tra email của bạn để xác thực tài khoản." });
    }

    // POST: api/account/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            return Unauthorized(new { Message = "Email chưa được xác thực hoặc tài khoản không tồn tại" });
        }
        if (!user.EmailConfirmed)
        {
            return Unauthorized(new { Message = "Email chưa được xác thực hoặc tài khoản không tồn tại" });
        }

        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            return Unauthorized(new { Message = "Email hoặc mật khẩu không hợp lệ." });
        }

        user.LastLoginAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        var roles = await _userManager.GetRolesAsync(user);

        return Ok(new
        {
            Message = "Đăng nhập thành công!",
            Token = _tokenService.CreateToken(user, roles)
        });
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(int userId, string token)
    {
        if (userId == default || string.IsNullOrEmpty(token))
        {
            return BadRequest("Dữ liệu không hợp lệ.");
        }

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return NotFound("Không tìm thấy người dùng.");
        }

        // Xác thực token
        var result = await _userManager.ConfirmEmailAsync(user, token);

        if (result.Succeeded)
        {
            // Khi thành công, bạn có thể trả về một trang HTML đơn giản hoặc redirect đến trang đăng nhập của frontend
            return Ok("Xác thực email thành công! Bây giờ bạn có thể đăng nhập.");
        }

        return BadRequest("Xác thực email thất bại.");
    }
}