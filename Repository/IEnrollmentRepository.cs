using FirstAidAPI.DTO;
using FirstAidAPI.Models;

namespace FirstAidAPI.Repository
{
    public interface IEnrollmentRepository
    {
        Task<CourseEnrollment?> GetByIdAsync(int id);

        Task<CourseEnrollment?> GetByUserAndCourseAsync(int userId, int courseId);

        Task<List<CourseEnrollment>> GetByUserIdAsync(int userId);

        Task<List<CourseEnrollment>> GetByCourseIdAsync(int courseId);

        Task<bool> ExistsAsync(int userId, int courseId);

        Task<CourseEnrollment> AddAsync(CourseEnrollment enrollment);

        Task UpdateAsync(CourseEnrollment enrollment);

        Task DeleteAsync(int id);

        Task<int> CountByCourseIdAsync(int courseId);

        Task<PagedResult<CourseEnrollment>> GetUserEnrollmentsAsync(int userId, int page, int pageSize);
    }
}