using FirstAidAPI.DTO;
using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IPracticalCourseRepository
    {
        Task<IEnumerable<PracticalCourse>> GetAllAsync();

        Task<PracticalCourse?> GetByIdAsync(int id);

        Task<PracticalCourse> CreateAsync(PracticalCourse practicalCourse);

        Task<bool> DeleteAsync(int id);

        Task<PracticalCourse> UpdateAsync(PracticalCourse practicalCourse);

        Task<List<PracticalCourse>> GetByIdsWithLockAsync(List<int> courseIds);

        Task<List<PracticalCourse>> GetByIdsAsync(List<int> courseIds);

        Task<PagedResult<PracticalCourse>> GetPagedAsync(int page, int pageSize, string? search);
    }
}