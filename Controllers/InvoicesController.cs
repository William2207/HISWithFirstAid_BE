using FirstAidAPI.DTO.Invoice;
using FirstAidAPI.DTO.Payment;
using FirstAidAPI.Enums;
using FirstAidAPI.Repository;
using FirstAidAPI.Service;
using FirstAidAPI.Service.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IMomoService _momoService;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<InvoicesController> _logger;

        public InvoicesController(
            IInvoiceService invoiceService,
            IMomoService momoService,
            IInvoiceRepository invoiceRepository,
            IAppointmentRepository appointmentRepository,
            IConfiguration configuration,
            ILogger<InvoicesController> logger)
        {
            _invoiceService = invoiceService;
            _momoService = momoService;
            _invoiceRepository = invoiceRepository;
            _appointmentRepository = appointmentRepository;
            _configuration = configuration;
            _logger = logger;
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
            var frontendUrl = _configuration["Frontend:Url"];

            try
            {
                if (!_momoService.ValidateSignature(callback))
                {
                    return Redirect($"{frontendUrl}/payment/failed?message=Invalid+signature");
                }

                // OrderId của Momo chính là InvoiceNumber
                var invoice = await _invoiceRepository.GetByInvoiceNumberAsync(callback.OrderId);
                if (invoice == null)
                {
                    return Redirect($"{frontendUrl}/payment/failed?message=Invoice+not+found");
                }

                if (callback.ResultCode == 0)
                {
                    invoice.Status = OrderStatus.Completed;
                    invoice.PaidAmount = invoice.Total;
                    invoice.RemainingAmount = 0;
                    invoice.PaidAt = DateTime.UtcNow;
                    await _invoiceRepository.UpdateAsync(invoice);

                    _logger.LogInformation("Invoice {InvoiceNumber} paid successfully. TransId: {TransId}",
                        invoice.InvoiceNumber, callback.TransId);

                    return Redirect($"{frontendUrl}/patient?tab=payments&status=success&invoiceNumber={invoice.InvoiceNumber}");
                }
                else
                {
                    // Thanh toán thất bại → hủy appointment nếu có
                    invoice.Status = OrderStatus.Cancelled;
                    await _invoiceRepository.UpdateAsync(invoice);

                    if (invoice.AppointmentId.HasValue)
                    {
                        var appointment = await _appointmentRepository.GetByIdAsync(invoice.AppointmentId.Value);
                        if (appointment != null)
                        {
                            await _appointmentRepository.DeleteAsync(appointment);
                            _logger.LogInformation("Cancelled appointment {AppointmentId} due to failed payment",
                                invoice.AppointmentId.Value);
                        }
                    }

                    return Redirect($"{frontendUrl}/payment/failed?invoiceNumber={callback.OrderId}&message={callback.Message}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Momo callback for invoice");
                return Redirect($"{frontendUrl}/payment/failed?message={ex.Message}");
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

                var invoice = await _invoiceRepository.GetByInvoiceNumberAsync(callback.OrderId);
                if (invoice == null)
                {
                    return Ok(new { resultCode = 99, message = "Invoice not found" });
                }

                if (callback.ResultCode == 0)
                {
                    invoice.Status = OrderStatus.Completed;
                    invoice.PaidAmount = invoice.Total;
                    invoice.RemainingAmount = 0;
                    invoice.PaidAt = DateTime.UtcNow;
                    await _invoiceRepository.UpdateAsync(invoice);
                }
                else
                {
                    invoice.Status = OrderStatus.Cancelled;
                    await _invoiceRepository.UpdateAsync(invoice);

                    // Hủy appointment khi payment fail
                    if (invoice.AppointmentId.HasValue)
                    {
                        var appointment = await _appointmentRepository.GetByIdAsync(invoice.AppointmentId.Value);
                        if (appointment != null)
                        {
                            await _appointmentRepository.DeleteAsync(appointment);
                        }
                    }
                }

                return Ok(new { resultCode = 0, message = "Success" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Momo IPN for invoice");
                return Ok(new { resultCode = 99, message = ex.Message });
            }
        }
    }
}
