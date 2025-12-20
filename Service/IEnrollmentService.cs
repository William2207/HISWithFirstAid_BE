using FirstAidAPI.DTO;
using FirstAidAPI.DTO.Enrollment;

namespace FirstAidAPI.Service
{
    public interface IEnrollmentService
    {
        Task CreateEnrollmentsFromOrderAsync(int orderId);

        Task<bool> IsUserEnrolledAsync(int userId, int courseId);

        Task<List<EnrollmentDto>> GetUserEnrollmentsAsync(int userId);

        Task<List<StudentDto>> GetCourseStudentsAsync(int courseId);

        Task AddReviewAsync(int enrollmentId, int rating, string? review);

        Task<bool> ExistsAsync(int userId, int courseId);

        Task<PagedResult<EnrollmentDto>> GetUserEnrollmentsAsync(int userId, int page, int pageSize);
    }
}