using FirstAidAPI.DTO.Admission;
using FirstAidAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Nurse")]
    public class AdmissionsController : ControllerBase
    {
        private readonly IAdmissionService _admissionService;

        public AdmissionsController(IAdmissionService admissionService)
        {
            _admissionService = admissionService;
        }

        /// <summary>
        /// Lấy danh sách bệnh nhân bác sĩ đã chỉ định nhập viện nhưng chưa được gán giường.
        /// </summary>
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingAdmissions()
        {
            var pendingList = await _admissionService.GetPendingAdmissionsAsync();
            return Ok(pendingList);
        }

        /// <summary>
        /// Lấy danh sách giường đang trống để y tá lựa chọn.
        /// </summary>
        [HttpGet("available-beds")]
        public async Task<IActionResult> GetAvailableBeds()
        {
            var beds = await _admissionService.GetAvailableBedsAsync();
            return Ok(beds);
        }

        /// <summary>
        /// Gán giường cho bệnh nhân và lưu lịch sử nhập viện.
        /// </summary>
        [HttpPost("assign-bed")]
        public async Task<IActionResult> AssignBed([FromBody] AssignBedRequest request)
        {
            var nurseUserId = GetCurrentUserId();
            if (nurseUserId is null)
                return Unauthorized(new { message = "Invalid token." });

            var admissionRecord = await _admissionService.AssignBedAsync(nurseUserId.Value, request);
            return Ok(admissionRecord);
        }

        /// <summary>
        /// Xuất viện: giải phóng giường và đánh dấu thời gian xuất viện.
        /// </summary>
        [HttpPost("discharge/{patientId:int}")]
        public async Task<IActionResult> DischargePatient(int patientId)
        {
            await _admissionService.DischargePatientAsync(patientId);
            return Ok(new { message = $"Patient {patientId} discharged successfully." });
        }

        // ─── Helpers ────────────────────────────────────────────────────────────
        private int? GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out var id) ? id : null;
        }
    }
}
