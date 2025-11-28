using FirstAidAPI.Repository;
using FirstAidAPI.DTO.PracticalCourse;
using FirstAidAPI.Mappings;
using AutoMapper;

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
    }
}