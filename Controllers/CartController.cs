using Microsoft.AspNetCore.Mvc;
using FirstAidAPI.Service;
using FirstAidAPI.DTO.Cart;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetOrCreateCartByUserIdAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid token" });
            }
            try
            {
                var cart = await _cartService.GetOrCreateCartByUserIdAsync(userId);
                return Ok(new { success = true, data = cart });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching cart for user {UserId}", userId);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("item/add")]
        [Authorize]
        public async Task<IActionResult> AddCartItemAsync([FromBody] AddCartItemRequestDto cartItem)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid token" });
            }
            try
            {
                var newCartItem = await _cartService.AddCartItemAsync(userId, cartItem);
                return Ok(new { success = true, message = "Item added to cart successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item to cart");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("item/{cartItemId}")]
        [Authorize]
        public async Task<IActionResult> RemoveCartItemAsync(int cartItemId)
        {
            try
            {
                await _cartService.RemoveCartItemAsync(cartItemId);
                return Ok(new { success = true, message = "Item removed from cart successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item from cart");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("item/update")]
        [Authorize]
        public async Task<IActionResult> UpdateCartItemAsync([FromBody] CartItemResponseDto cartItem)
        {
            try
            {
                await _cartService.UpdateCartItemAsync(cartItem);
                return Ok(new { success = true, message = "Cart item updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart item");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}