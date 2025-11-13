using FirstAidAPI.DTO;
using FirstAidAPI.Service;
using Microsoft.AspNetCore.Authorization;
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

    public UsersController(IUserService userService, IPhotoService photoService)
    {
        _userService = userService;
        _photoService = photoService;
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
        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
            Avatar = user.Avatar,
            DateOfBirth = user.DateOfBirth,
            Role = user.Role,
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

        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
            Avatar = user.Avatar,
            DateOfBirth = user.DateOfBirth,
            Role = user.Role,
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

    [HttpPost("me/avatar")]
    [Authorize]
    public async Task<IActionResult> AddAvatar(IFormFile file)
    {
        // 1. Lấy user ID từ token để đảm bảo đúng là người dùng đang đăng nhập
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            return Unauthorized(new { message = "Invalid token" });
        }

        // 2. Lấy thông tin người dùng từ database
        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        // 3. Sử dụng PhotoService để tải file lên Cloudinary
        var uploadResult = await _photoService.AddPhotoAsync(file);

        // 4. Kiểm tra kết quả trả về từ Cloudinary
        if (uploadResult.Error != null)
        {
            return BadRequest(new { message = uploadResult.Error.Message });
        }

        // 5. Cập nhật URL avatar mới cho người dùng
        user.Avatar = uploadResult.SecureUrl.AbsoluteUri;

        // 6. Lưu thay đổi vào database
        // Lưu ý: Bạn cần có một phương thức để cập nhật thông tin người dùng trong IUserService
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
            ScenarioName = sp.Scenario.Name,
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

    // Các endpoints khác cho Get All, Create, Update, Delete...
}