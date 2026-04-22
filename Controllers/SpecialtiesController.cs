using FirstAidAPI.DTO.Specialty;
using FirstAidAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SpecialtiesController : ControllerBase
    {
        private readonly ISpecialtyService _specialtyService;

        public SpecialtiesController(ISpecialtyService specialtyService)
        {
            _specialtyService = specialtyService;
        }

        /// <summary>Danh sách chuyên khoa dùng cho dropdown (không cần phân trang)</summary>
        [HttpGet("lookup")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSpecialtiesForLookup()
        {
            var specialties = await _specialtyService.GetSpecialtiesForLookupAsync();
            return Ok(specialties);
        }

        /// <summary>Danh sách chuyên khoa có phân trang và tìm kiếm (Admin only)</summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSpecialties(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null)
        {
            var (items, totalCount) = await _specialtyService.GetPagedSpecialtiesAsync(page, pageSize, search);

            return Ok(new
            {
                data = items,
                currentPage = page,
                pageSize,
                totalItems = totalCount,
                totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            });
        }

        /// <summary>Chi tiết một chuyên khoa (Admin only)</summary>
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSpecialtyById(int id)
        {
            var specialty = await _specialtyService.GetSpecialtyByIdAsync(id);
            return Ok(specialty);
        }

        /// <summary>Tạo chuyên khoa mới (Admin only)</summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSpecialty([FromBody] CreateSpecialtyRequest request)
        {
            var created = await _specialtyService.CreateSpecialtyAsync(request);
            return CreatedAtAction(nameof(GetSpecialtyById), new { id = created.Id }, created);
        }

        /// <summary>Cập nhật chuyên khoa (Admin only)</summary>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSpecialty(int id, [FromBody] UpdateSpecialtyRequest request)
        {
            var updated = await _specialtyService.UpdateSpecialtyAsync(id, request);
            return Ok(updated);
        }

        /// <summary>Xóa mềm chuyên khoa — đặt IsActive = false (Admin only)</summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSpecialty(int id)
        {
            await _specialtyService.DeleteSpecialtyAsync(id);
            return NoContent();
        }
    }
}
