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

            // Map từ CreateDto sang Entity
            CreateMap<CreatePracticalCourseDto, PracticalCourse>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Set trong Service
                .ForMember(dest => dest.EnrolledStudents, opt => opt.Ignore()); // Mặc định = 0

            // Map từ UpdateDto sang Entity (chỉ map các field có giá trị)
            CreateMap<UpdatePracticalCourseDto, PracticalCourse>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.EnrolledStudents, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}