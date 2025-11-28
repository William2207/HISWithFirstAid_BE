using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IPracticalCourseRepository
    {
        public Task<IEnumerable<PracticalCourse>> GetAllAsync();

        public Task<PracticalCourse?> GetByIdAsync(int id);
    }
}