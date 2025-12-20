using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace FirstAidAPI.Service
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);

        Task<DeletionResult> DeletePhotoAsync(string publicId);

        Task<VideoUploadResult> AddVideoAsync(IFormFile file);

        Task<DeletionResult> DeleteVideoAsync(string publicId);

        // Upload tự động (MỚI)
        Task<RawUploadResult> AddMediaAsync(IFormFile file);

        // Lấy thông tin (MỚI)
        Task<GetResourceResult> GetVideoInfoAsync(string publicId);
    }
}