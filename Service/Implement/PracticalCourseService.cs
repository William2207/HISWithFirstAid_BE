using AutoMapper;
using FirstAidAPI.DTO;
using FirstAidAPI.DTO.PracticalCourse;
using FirstAidAPI.Mappings;
using FirstAidAPI.Models;
using FirstAidAPI.Repository;

namespace FirstAidAPI.Service.Implement
{
    public class PracticalCourseService : IPracticalCourseService
    {
        private readonly IPracticalCourseRepository _practicalCourseRepository;
        private readonly IMapper _mapper;

        public PracticalCourseService(IPracticalCourseRepository practicalCourseRepository, IMapper mapper)
        {
            _practicalCourseRepository = practicalCourseRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PracticalCourseDto>> GetAllPracticalCoursesAsync()
        {
            var practicalCourses = await _practicalCourseRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PracticalCourseDto>>(practicalCourses);
        }

        public async Task<PracticalCourseDto?> GetPracticalCourseByIdAsync(int id)
        {
            var practicalCourse = await _practicalCourseRepository.GetByIdAsync(id);
            if (practicalCourse == null)
                return null;
            return _mapper.Map<PracticalCourseDto>(practicalCourse);
        }

        public async Task<PracticalCourseDto> CreateCourseAsync(CreatePracticalCourseDto dto)
        {
            // Validation
            ValidateCourseDto(dto);

            var course = _mapper.Map<PracticalCourse>(dto);
            course.CreatedAt = DateOnly.FromDateTime(DateTime.Now);

            var created = await _practicalCourseRepository.CreateAsync(course);
            return _mapper.Map<PracticalCourseDto>(created);
        }

        public async Task<PracticalCourseDto?> UpdateCourseAsync(int id, UpdatePracticalCourseDto dto)
        {
            var course = await _practicalCourseRepository.GetByIdAsync(id);
            if (course == null) return null;

            // Map các giá trị mới vào course hiện tại (chỉ cập nhật các field không null)
            _mapper.Map(dto, course);

            // Validation ngày tháng
            if (course.EndDate < course.StartDate)
            {
                throw new ArgumentException("End date must be after start date");
            }

            var updated = await _practicalCourseRepository.UpdateAsync(course);
            return updated == null ? null : _mapper.Map<PracticalCourseDto>(updated);
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            return await _practicalCourseRepository.DeleteAsync(id);
        }

        public async Task<bool> PublishCourseAsync(int id)
        {
            var course = await _practicalCourseRepository.GetByIdAsync(id);
            if (course == null) return false;

            course.IsPublished = true;
            var updated = await _practicalCourseRepository.UpdateAsync(course);
            return updated != null;
        }

        public async Task<bool> UnpublishCourseAsync(int id)
        {
            var course = await _practicalCourseRepository.GetByIdAsync(id);
            if (course == null) return false;

            course.IsPublished = false;
            var updated = await _practicalCourseRepository.UpdateAsync(course);
            return updated != null;
        }

        public async Task<PagedResult<PracticalCourseDto>> GetPagedCoursesAsync(int page, int pageSize, string? search)
        {
            var pagedCourses = await _practicalCourseRepository.GetPagedAsync(page, pageSize, search);

            return new PagedResult<PracticalCourseDto>
            {
                Data = _mapper.Map<List<PracticalCourseDto>>(pagedCourses.Data),
                CurrentPage = pagedCourses.CurrentPage,
                PageSize = pagedCourses.PageSize,
                TotalItems = pagedCourses.TotalItems,
                TotalPages = pagedCourses.TotalPages
            };
        }

        private void ValidateCourseDto(CreatePracticalCourseDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Title is required");

            if (dto.Price < 0)
                throw new ArgumentException("Price must be greater than or equal to 0");

            if (dto.DurationMinutes <= 0)
                throw new ArgumentException("Duration must be greater than 0");

            if (dto.EndDate < dto.StartDate)
                throw new ArgumentException("End date must be after start date");

            if (dto.MaxStudents <= 0)
                throw new ArgumentException("Max students must be greater than 0");

            // Validation cho Highlights và Requirements
            if (dto.Highlights != null && dto.Highlights.Any(h => string.IsNullOrWhiteSpace(h)))
                throw new ArgumentException("Highlights cannot contain empty values");

            if (dto.Requirements != null && dto.Requirements.Any(r => string.IsNullOrWhiteSpace(r)))
                throw new ArgumentException("Requirements cannot contain empty values");
        }
    }
}
