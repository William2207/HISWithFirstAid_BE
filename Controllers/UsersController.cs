using FirstAidAPI.DTO;
using FirstAidAPI.DTO.Scenario;
using FirstAidAPI.DTO.Technique;
using FirstAidAPI.DTO.User;
using FirstAidAPI.Models;
using FirstAidAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IPhotoService _photoService;
    private readonly UserManager<User> _userManager;

    public UsersController(IUserService userService, IPhotoService photoService, UserManager<User> userManager)
    {
        _userService = userService;
        _photoService = photoService;
        _userManager = userManager;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        if (user.Email == null)
        {
            return BadRequest("User email is required");
        }
        var roles = await _userManager.GetRolesAsync(user);

        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
            Avatar = user.Avatar,
            DateOfBirth = user.DateOfBirth,
            Role = roles,
            CreatedAt = user.CreatedAt,
            LastLoginAt = user.LastLoginAt
        };

        return Ok(userDto);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //Console.WriteLine("UserId from token: " + userIdClaim);
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            return Unauthorized(new { message = "Invalid token" });
        }

        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }
        if (user.Email == null)
        {
            return BadRequest("User email is required");
        }
        var roles = await _userManager.GetRolesAsync(user);

        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
            Avatar = user.Avatar,
            DateOfBirth = user.DateOfBirth,
            Role = roles,
            CreatedAt = user.CreatedAt,
            LastLoginAt = user.LastLoginAt
        };

        return Ok(userDto);
    }

    [HttpPut("me")]
    [Authorize]
    public async Task<IActionResult> UpdateCurrentUser([FromBody] UpdateUserDto updateUserDto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            return Unauthorized(new { message = "Invalid token" });
        }
        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }
        // Cập nhật thông tin người dùng
        user.FullName = updateUserDto.FullName ?? user.FullName;
        user.PhoneNumber = updateUserDto.PhoneNumber ?? user.PhoneNumber;
        user.Email = updateUserDto.Email ?? user.Email;
        var updateResult = await _userService.UpdateUserAsync(userId, user);
        if (updateResult)
        {
            return Ok(new { message = "User updated successfully." });
        }
        return BadRequest(new { message = "Failed to update user." });
    }

    [HttpPut("me/password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] UpdatePasswordDto changePasswordDto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            return Unauthorized(new { message = "Invalid token" });
        }
        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }
        var result = await _userService.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
        if (result)
        {
            return Ok(new { message = "Password changed successfully." });
        }
        return BadRequest(new { message = "Failed to change password." });
    }

    [HttpPost("me/avatar")]
    [Authorize]
    public async Task<IActionResult> AddAvatar(IFormFile file)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            return Unauthorized(new { message = "Invalid token" });
        }

        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        var uploadResult = await _photoService.AddPhotoAsync(file);

        if (uploadResult.Error != null)
        {
            return BadRequest(new { message = uploadResult.Error.Message });
        }

        user.Avatar = uploadResult.SecureUrl.AbsoluteUri;

        var updateResult = await _userService.UpdateUserAsync(userId, user);

        if (updateResult)
        {
            return Ok(new { avatarUrl = user.Avatar });
        }

        return BadRequest(new { message = "Failed to update avatar." });
    }

    [HttpGet("me/saved-techniques")]
    public async Task<IActionResult> GetMySavedTechniques()
    {
        if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            return Unauthorized(new { message = "Invalid token" });
        }

        var savedTechniques = await _userService.GetSavedTechniquesByUserIdAsync(userId);

        // Map từ Model sang DTO, chỉ lấy những thông tin cần thiết
        var savedTechniquesDto = savedTechniques.Select(st => new SavedTechniqueDto
        {
            Id = st.Id,
            Name = st.Technique.Name,
            TypeId = st.Technique.TechniqueTypeId,
            TypeName = st.Technique.Type.Name,
            Difficulty = st.Technique.Difficulty // Giả sử Model Technique có thuộc tính Difficulty
        });

        return Ok(savedTechniquesDto);
    }

    [HttpGet("me/scenario-attempts")]
    public async Task<IActionResult> GetMyScenarioAttempts([FromQuery] int limit = 10)
    {
        if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            return Unauthorized(new { message = "Invalid token" });
        }

        var scenarioAttempts = await _userService.GetScenarioAttemptsByUserIdAsync(userId, limit);

        var scenarioAttemptsDto = scenarioAttempts.Select(sa => new ScenarioAttemptDto
        {
            Id = sa.Id,
            ScenarioName = sa.Scenario.Name,
            Score = sa.Score,
            IsPassed = sa.IsPassed,
            AttemptedAt = sa.AttemptedAt
        });

        return Ok(scenarioAttemptsDto);
    }

    [HttpGet("me/progress/scenarios")]
    public async Task<IActionResult> GetMyScenarioProgresses()
    {
        if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            return Unauthorized(new { message = "Invalid token" });
        }

        var scenarioProgresses = await _userService.GetScenarioProgressesByUserIdAsync(userId);

        var scenarioProgressesDto = scenarioProgresses.Select(sp => new UserScenarioProgressDto
        {
            ScenarioId = sp.ScenarioId,
            ScenarioName = sp.Scenario.Title,
            IsCompleted = sp.Status, // Map từ thuộc tính Status của Model
            HighestScore = sp.HighestScore,
            LastAccessedAt = sp.LastAccessedAt
        });

        return Ok(scenarioProgressesDto);
    }

    [HttpGet("me/progress/techniques")]
    public async Task<IActionResult> GetMyTechniqueProgresses()
    {
        if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            return Unauthorized(new { message = "Invalid token" });
        }

        var techniqueProgresses = await _userService.GetTechniqueProgressesByUserIdAsync(userId);

        var techniqueProgressesDto = techniqueProgresses.Select(tp => new UserTechniqueProgressDto
        {
            TechniqueId = tp.TechniqueId,
            TechniqueName = tp.Technique.Name,
            IsCompleted = tp.Status, // Map từ thuộc tính Status của Model
            LastAccessedAt = tp.LastAccessedAt
        });

        return Ok(techniqueProgressesDto);
    }
}
