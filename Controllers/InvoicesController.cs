using System;
using FirstAidAPI.DTO.Invoice;
using FirstAidAPI.DTO.Payment;
using FirstAidAPI.Enums;
using FirstAidAPI.Service;
using FirstAidAPI.Service.Payment;
using FirstAidAPI.Repository;
using FirstAidAPI.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : Controller
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IMomoService _momoService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<InvoicesController> _logger;

        public InvoicesController(
            IInvoiceService invoiceService,
            IMomoService momoService,
            IConfiguration configuration,
            ILogger<InvoicesController> logger)
        {
            _invoiceService = invoiceService;
            _momoService = momoService;
            _configuration = configuration;
            _logger = logger;
        }

        private int GetCurrentUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                throw new UnauthorizedException("Không tìm thấy thông tin xác thực.");
            }
            return userId;
        }

        /// <summary>Tạo hóa đơn + trả về Momo payment URL</summary>
        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceDto createInvoiceDto)
        {
            try
            {
                var result = await _invoiceService.CreateInvoiceAsync(createInvoiceDto);
                return Ok(new { success = true, data = result });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating invoice");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        /// <summary>Momo redirect callback – người dùng được chuyển về đây sau thanh toán</summary>
        [HttpGet("momo-callback")]
        public async Task<IActionResult> MomoCallback([FromQuery] MomoCallbackDto callback)
        {
            var frontendUrl = _configuration["Frontend:HISUrl"] ?? _configuration["Frontend:Url"];

            try
            {
                if (!_momoService.ValidateSignature(callback))
                {
                    return Redirect($"{frontendUrl}/payment/failed?message=Invalid+signature");
                }

                // Xử lý kết quả thanh toán qua Service (Clean Code)
                await _invoiceService.ProcessMomoPaymentAsync(callback);

                string redirectUrl;
                if (callback.ResultCode == 0)
                {
                    redirectUrl = $"{frontendUrl}/patient?tab=payments&status=success&invoiceNumber={callback.OrderId}";
                }
                else
                {
                    redirectUrl = $"{frontendUrl}/payment/failed?invoiceNumber={callback.OrderId}&message={callback.Message}";
                }

                _logger.LogInformation("Redirecting browser to: {RedirectUrl}", redirectUrl);
                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Momo callback for invoice {InvoiceNumber}", callback.OrderId);
                return Redirect($"{frontendUrl}/payment/failed?invoiceNumber={callback.OrderId}&message={Uri.EscapeDataString(ex.Message)}");
            }
        }

        /// <summary>Momo IPN – Momo gọi trực tiếp vào đây (không qua browser)</summary>
        [HttpPost("momo-ipn")]
        [Consumes("application/json")]
        public async Task<IActionResult> MomoIPN([FromBody] MomoCallbackDto callback)
        {
            try
            {
                if (!_momoService.ValidateSignature(callback))
                {
                    return Ok(new { resultCode = 97, message = "Invalid signature" });
                }

                // Xử lý IPN qua Service (Clean Code: không gọi trực tiếp db layer)
                await _invoiceService.ProcessMomoPaymentAsync(callback);

                return Ok(new { resultCode = 0, message = "Success" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Momo IPN for invoice {InvoiceNumber}", callback.OrderId);
                return Ok(new { resultCode = 99, message = ex.Message });
            }
        }

        [HttpGet("patient/me")]
        [Authorize]
        public async Task<IActionResult> GetMyInvoices()
        {
            try
            {
                var userId = GetCurrentUserId();
                var invoices = await _invoiceService.GetInvoicesByUserIdAsync(userId);
                return Ok(invoices);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
