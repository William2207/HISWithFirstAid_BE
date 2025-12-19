using FirstAidAPI.DTO;
using FirstAidAPI.DTO.PracticalCourse;

namespace FirstAidAPI.Service
{
    public interface IPracticalCourseService
    {
        public Task<IEnumerable<PracticalCourseDto>> GetAllPracticalCoursesAsync();

        public Task<PracticalCourseDto?> GetPracticalCourseByIdAsync(int id);

        Task<PracticalCourseDto> CreateCourseAsync(CreatePracticalCourseDto dto);

        Task<PracticalCourseDto?> UpdateCourseAsync(int id, UpdatePracticalCourseDto dto);

        Task<bool> DeleteCourseAsync(int id);

        Task<bool> PublishCourseAsync(int id);

        Task<bool> UnpublishCourseAsync(int id);

        Task<PagedResult<PracticalCourseDto>> GetPagedCoursesAsync(int page, int pageSize, string? search);
    }
}