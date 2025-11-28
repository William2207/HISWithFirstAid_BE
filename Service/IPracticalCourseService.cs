using FirstAidAPI.DTO.PracticalCourse;

namespace FirstAidAPI.Service
{
    public interface IPracticalCourseService
    {
        public Task<IEnumerable<PracticalCourseDto>> GetAllPracticalCoursesAsync();

        public Task<PracticalCourseDto?> GetPracticalCourseByIdAsync(int id);
    }
}