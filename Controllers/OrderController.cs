using FirstAidAPI.DTO.Order;
using FirstAidAPI.DTO.Payment;
using FirstAidAPI.Service;
using FirstAidAPI.Service.Payment;
using Microsoft.AspNetCore.Mvc;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IMomoService _momoService;
        private readonly IConfiguration _configuration;

        public OrderController(IOrderService orderService, IMomoService momoService, IConfiguration configuration)
        {
            _orderService = orderService;
            _momoService = momoService;
            _configuration = configuration;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                {
                    return NotFound(new { success = false, message = "Order not found" });
                }

                return Ok(new { success = true, data = order });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserOrders(int userId)
        {
            try
            {
                var orders = await _orderService.GetByUserIdAsync(userId);
                return Ok(new { success = true, data = orders });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            try
            {
                var result = await _orderService.CreateOrderAsync(createOrderDto);

                return Ok(new
                {
                    success = true,
                    message = "Tạo đơn hàng thành công",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // Return URL - User được redirect về đây
        [HttpGet("momo-callback")]
        public async Task<IActionResult> MomoCallback([FromQuery] MomoCallbackDto callback)
        {
            try
            {
                if (!_momoService.ValidateSignature(callback))
                {
                    return Redirect($"{_configuration["Frontend:Url"]}/payment/failed?message=Invalid+signature");
                }

                var order = await _orderService.GetByOrderNumberAsync(callback.OrderId);
                if (order == null)
                {
                    return Redirect($"{_configuration["Frontend:Url"]}/payment/failed?message=Order+not+found");
                }

                if (callback.ResultCode == 0)
                {
                    await _orderService.CompleteOrderAsync(order.Id, callback.TransId.ToString());
                    return Redirect($"{_configuration["Frontend:Url"]}/payment/success?orderNumber={callback.OrderId}");
                }
                else
                {
                    await _orderService.FailOrderAsync(order.Id);
                    return Redirect($"{_configuration["Frontend:Url"]}/payment/failed?orderNumber={callback.OrderId}&message={callback.Message}");
                }
            }
            catch (Exception ex)
            {
                return Redirect($"{_configuration["Frontend:Url"]}/payment/failed?message={ex.Message}");
            }
        }

        // IPN URL - Momo gọi API này (không qua browser)
        [HttpPost("momo-ipn")]
        [Consumes("application/json")]
        public async Task<IActionResult> MomoIPN([FromBody] MomoCallbackDto callback)
        {
            try
            {
                // ✅ Dùng _momoService đã inject
                if (!_momoService.ValidateSignature(callback))
                {
                    return Ok(new { resultCode = 97, message = "Invalid signature" });
                }

                var order = await _orderService.GetByOrderNumberAsync(callback.OrderId);
                if (order == null)
                {
                    return Ok(new { resultCode = 99, message = "Order not found" });
                }

                if (callback.ResultCode == 0)
                {
                    await _orderService.CompleteOrderAsync(order.Id, callback.TransId.ToString());
                }
                else
                {
                    await _orderService.FailOrderAsync(order.Id);
                }

                return Ok(new { resultCode = 0, message = "Success" });
            }
            catch (Exception ex)
            {
                return Ok(new { resultCode = 99, message = ex.Message });
            }
        }
    }
}