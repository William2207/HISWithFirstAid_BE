using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IPracticalCourseRepository
    {
        Task<IEnumerable<PracticalCourse>> GetAllAsync();

        Task<PracticalCourse?> GetByIdAsync(int id);

        Task<PracticalCourse> UpdateAsync(PracticalCourse practicalCourse);

        Task<List<PracticalCourse>> GetByIdsWithLockAsync(List<int> courseIds);

        Task<List<PracticalCourse>> GetByIdsAsync(List<int> courseIds);
    }
}