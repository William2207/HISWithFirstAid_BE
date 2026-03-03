using FirstAidAPI.DTO.Queue;
using FirstAidAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FirstAidAPI.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueController : Controller
    {
        private readonly IQueueService _queueService;
        private readonly IUserService _userService;

        public QueueController(IQueueService queueService, IUserService userService)
        {
            _queueService = queueService;
            _userService = userService;
        }

        [HttpPost("issue")]
        [AllowAnonymous]
        public async Task<ActionResult<QueueDTO>> IssueQueue()
        {
            var queue = await _queueService.IssueQueueAsync();
            return Ok(queue);
        }

        [HttpPost("call-next")]
        [Authorize(Roles = "Receptionist")]
        public async Task<ActionResult<QueueDTO>> CallNext()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("Không tìm thấy thông tin người dùng.");
            }
            if (!int.TryParse(userIdString, out var receptionistId))
            {
                return BadRequest("User ID không hợp lệ.");
            }
            var queue = await _queueService.CallNextQueueAsync(receptionistId);
            return Ok(queue);
        }
    }
}
