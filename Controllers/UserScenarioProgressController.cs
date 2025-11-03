using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FirstAidAPI.Service;
using FirstAidAPI.DTO;
using FirstAidAPI.Models;
using System.Security.Claims;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserScenarioProgressController : Controller
    {
        private readonly IUserScenarioProgressService _userScenarioProgressService;

        public UserScenarioProgressController(IUserScenarioProgressService userScenarioProgressService)
        {
            _userScenarioProgressService = userScenarioProgressService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SaveProgress([FromBody] UserScenarioProgressDto requestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("Không tìm thấy thông tin người dùng.");
            }
            if (!int.TryParse(userIdString, out var userId))
            {
                return BadRequest("User ID không hợp lệ.");
            }

            int scenarioId = requestDto.ScenarioId;
            int currentScore = requestDto.HighestScore;
            var result = await _userScenarioProgressService.SaveCompletionProgressAsync(userId, scenarioId, currentScore);

            return Ok();
        }
    }
}