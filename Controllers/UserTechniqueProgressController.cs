using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Cần thêm để sử dụng [Authorize]
using FirstAidAPI.Service;
using System.Security.Claims; // Cần thêm để làm việc với Claims
using System.Threading.Tasks;
using FirstAidAPI.DTO.Technique;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserTechniqueProgressController : Controller
    {
        private readonly IUserTechniqueProgressService _progressService;

        public UserTechniqueProgressController(IUserTechniqueProgressService progressService)
        {
            _progressService = progressService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SaveProgress([FromBody] UserTechniqueProgressDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // --- LẤY USER ID TỪ TOKEN MỘT CÁCH AN TOÀN ---
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                // Điều này không nên xảy ra nếu có [Authorize], nhưng là một bước kiểm tra an toàn
                return Unauthorized("Không tìm thấy thông tin người dùng.");
            }

            if (!int.TryParse(userIdString, out var userId))
            {
                return BadRequest("User ID không hợp lệ.");
            }
            // ---------------------------------------------

            // Lấy TechniqueId từ DTO mà client gửi lên
            int techniqueId = requestDto.TechniqueId;

            // Các thuộc tính khác trong DTO (TechniqueName, IsCompleted, LastAccessedAt)
            // sẽ bị bỏ qua ở đây, vì server sẽ tự quyết định các giá trị đó khi lưu.

            // Gọi service chỉ với các thông tin cần thiết và đáng tin cậy
            var result = await _progressService.SaveCompletionProgressAsync(userId, techniqueId);

            if (!result)
            {
                return BadRequest(new { message = "Không thể lưu tiến độ." });
            }

            return Ok(new { message = "Lưu tiến độ thành công." });
        }
    }
}