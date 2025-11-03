using FirstAidAPI.DTO;
using FirstAidAPI.Models;
using FirstAidAPI.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstAidAPI.Service.Implement
{
    public class TechniqueService : ITechniqueService
    {
        private readonly ITechniqueRepository _techniqueRepository;

        // Inject ITechniqueRepository vào constructor
        public TechniqueService(ITechniqueRepository techniqueRepository)
        {
            _techniqueRepository = techniqueRepository;
        }

        public async Task<IEnumerable<Technique>> GetAllTechniquesAsync()
        {
            return await _techniqueRepository.GetAllAsync();
        }

        public async Task<PagedResult<Technique>> GetTechniquesAsync(int page, int pageSize, List<string>? difficulties, List<string>? types, string? search)
        {
            // Business logic/validation được chuyển về đây
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 9;
            if (pageSize > 100) pageSize = 100; // Giới hạn page size là một business rule

            return await _techniqueRepository.GetAllFilteredAndPagedAsync(page, pageSize, difficulties, types, search);
        }

        public async Task<Technique?> GetTechniqueByIdAsync(int id)
        {
            return await _techniqueRepository.GetByIdAsync(id);
        }

        public async Task<Technique> CreateTechniqueAsync(Technique technique)
        {
            // Đây là nơi bạn có thể thêm business logic, ví dụ:
            // - Kiểm tra xem tên technique đã tồn tại chưa
            // - Validate dữ liệu trước khi thêm

            await _techniqueRepository.AddAsync(technique);
            await _techniqueRepository.SaveChangesAsync();
            return technique;
        }

        public async Task<bool> UpdateTechniqueAsync(int id, Technique technique)
        {
            // Lấy đối tượng hiện có từ DB
            var existingTechnique = await _techniqueRepository.GetByIdAsync(id);
            if (existingTechnique == null)
            {
                return false; // Trả về false nếu không tìm thấy
            }

            // Cập nhật các thuộc tính
            // Đây là nơi bạn có thể thêm logic, ví dụ không cho phép thay đổi một số trường
            existingTechnique.Name = technique.Name;
            existingTechnique.Title = technique.Title;
            existingTechnique.Description = technique.Description;
            existingTechnique.Type = technique.Type;
            existingTechnique.Difficulty = technique.Difficulty;
            existingTechnique.VideoUrl = technique.VideoUrl;
            existingTechnique.ImageUrl = technique.ImageUrl;
            existingTechnique.Duration = technique.Duration;
            existingTechnique.Icon = technique.Icon;
            // Cập nhật các danh sách liên quan nếu cần

            _techniqueRepository.Update(existingTechnique);
            return await _techniqueRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteTechniqueAsync(int id)
        {
            var techniqueToDelete = await _techniqueRepository.GetByIdAsync(id);
            if (techniqueToDelete == null)
            {
                return false; // Không tìm thấy để xóa
            }

            _techniqueRepository.Delete(techniqueToDelete);
            return await _techniqueRepository.SaveChangesAsync();
        }
    }
}