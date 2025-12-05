using AutoMapper;
using FirstAidAPI.DTO.Enrollment;
using FirstAidAPI.Models;

namespace FirstAidAPI.Mappings
{
    public class EnrollmentProfile : Profile
    {
        public EnrollmentProfile()
        {
            CreateMap<CourseEnrollment, EnrollmentDto>()
            .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.PracticalCourseId))
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.PracticalCourse.Title))
            .ForMember(dest => dest.CourseStartDate, opt => opt.MapFrom(src => src.PracticalCourse.StartDate))
            .ForMember(dest => dest.CourseEndDate, opt => opt.MapFrom(src => src.PracticalCourse.EndDate))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.PracticalCourse.Location));

            CreateMap<EnrollmentDto, CourseEnrollment>();
        }
    }
}