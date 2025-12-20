using FirstAidAPI.Service;
using Microsoft.AspNetCore.Mvc;

namespace FirstAidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : Controller
    {
        private readonly IPhotoService _photoService;

        public MediaController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpPost("upload-photo")]
        public async Task<IActionResult> UploadPhoto(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File không hợp lệ");

            // Validate
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/jpg", "image/gif" };
            if (!allowedTypes.Contains(file.ContentType.ToLower()))
                return BadRequest("Chỉ chấp nhận file ảnh (JPG, PNG, GIF)");

            if (file.Length > 5 * 1024 * 1024) // 5MB
                return BadRequest("Ảnh không được vượt quá 5MB");

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null)
                return BadRequest(result.Error.Message);

            return Ok(new
            {
                url = result.SecureUrl.ToString(),
                publicId = result.PublicId
            });
        }

        // Upload video (MỚI)
        [HttpPost("upload-video")]
        public async Task<IActionResult> UploadVideo(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File không hợp lệ");

            // Validate
            var allowedTypes = new[] { "video/mp4", "video/avi", "video/mov", "video/wmv" };
            if (!allowedTypes.Contains(file.ContentType.ToLower()))
                return BadRequest("Chỉ chấp nhận file video (MP4, AVI, MOV, WMV)");

            if (file.Length > 50 * 1024 * 1024) // 50MB
                return BadRequest("Video không được vượt quá 50MB");

            var result = await _photoService.AddVideoAsync(file);

            if (result.Error != null)
                return BadRequest(result.Error.Message);

            return Ok(new
            {
                url = result.SecureUrl.ToString(),
                publicId = result.PublicId,
                duration = result.Duration, // Độ dài video (giây)
                format = result.Format
            });
        }

        // Upload tự động phát hiện (MỚI)
        [HttpPost("upload")]
        public async Task<IActionResult> UploadMedia(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File không hợp lệ");

            try
            {
                var result = await _photoService.AddMediaAsync(file);

                if (result.Error != null)
                    return BadRequest(result.Error.Message);

                return Ok(new
                {
                    url = result.SecureUrl.ToString(),
                    publicId = result.PublicId,
                    resourceType = result.ResourceType // "image" hoặc "video"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Xóa video
        [HttpDelete("delete-video/{publicId}")]
        public async Task<IActionResult> DeleteVideo(string publicId)
        {
            var result = await _photoService.DeleteVideoAsync(publicId);

            if (result.Result == "ok")
                return Ok("Xóa video thành công");

            return BadRequest("Xóa video thất bại");
        }
    }
}