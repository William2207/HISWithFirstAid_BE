using AutoMapper;
using FirstAidAPI.DTO.PracticalCourse;
using FirstAidAPI.Models;

namespace FirstAidAPI.Mappings
{
    public class PracticalCourseMappingProfile : Profile
    {
        public PracticalCourseMappingProfile()
        {
            CreateMap<PracticalCourse, PracticalCourseDto>();
            CreateMap<CreatePracticalCourseDto, PracticalCourse>();
            //CreateMap<UpdatePracticalCourseDto, PracticalCourse>();
        }
    }
}