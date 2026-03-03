using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using FirstAidAPI.Configurations;

namespace FirstAidAPI.Service.Implement
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    // Tùy chọn: biến đổi ảnh trước khi lưu
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result;
        }

        public async Task<VideoUploadResult> AddVideoAsync(IFormFile file)
        {
            var uploadResult = new VideoUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new VideoUploadParams
                {
                    File = new FileDescription(file.FileName, stream),

                    // Tùy chọn: Giới hạn độ dài video (30 giây)
                    // EagerTransforms = new List<Transformation>
                    // {
                    //     new Transformation().Duration("30")
                    // },

                    // Tùy chọn: Nén video và cắt kích thước
                    Transformation = new Transformation()
                        .Width(1280)
                        .Height(720)
                        .Crop("limit")
                        .Quality("auto")
                        .FetchFormat("mp4"),

                    // Folder lưu trữ
                    Folder = "videos"
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        }

        public async Task<DeletionResult> DeleteVideoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Video // Quan trọng: phải chỉ định là video
            };
            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result;
        }

        public async Task<RawUploadResult> AddMediaAsync(IFormFile file)
        {
            var uploadResult = new RawUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();

                // Lấy phần đuôi file (extension)
                var extension = Path.GetExtension(file.FileName)?.ToLower();

                // Danh sách đuôi file hợp lệ
                var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp" };
                var videoExtensions = new[] { ".mp4", ".avi", ".mov", ".wmv", ".flv", ".mkv", ".webm" };

                var isImage = imageExtensions.Contains(extension);
                var isVideo = videoExtensions.Contains(extension);

                if (isVideo)
                {
                    var uploadParams = new VideoUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation()
                            .Width(1280)
                            .Height(720)
                            .Crop("limit")
                            .Quality("auto"),
                        Folder = "videos"
                    };
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }
                else if (isImage)
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation()
                            .Height(500)
                            .Width(500)
                            .Crop("fill")
                            .Gravity("face"),
                        Folder = "images"
                    };
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }
                else
                {
                    throw new Exception($"Chỉ hỗ trợ file ảnh ({string.Join(", ", imageExtensions)}) hoặc video ({string.Join(", ", videoExtensions)})");
                }
            }

            return uploadResult;
        }

        // BONUS: Lấy thông tin video (MỚI)
        public async Task<GetResourceResult> GetVideoInfoAsync(string publicId)
        {
            var result = await _cloudinary.GetResourceAsync(new GetResourceParams(publicId)
            {
                ResourceType = ResourceType.Video
            });
            return result;
        }
    }
}
