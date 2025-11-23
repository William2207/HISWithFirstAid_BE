using AutoMapper;
using FirstAidAPI.DTO.Quiz;
using FirstAidAPI.Models;

namespace FirstAidAPI.Mappings
{
    public class QuizQuestionMappingProfile : Profile
    {
        public QuizQuestionMappingProfile()
        {
            CreateMap<QuizQuestion, QuizQuestionDto>()
                .ForMember(dest => dest.TechniqueName,
                    opt => opt.MapFrom(src => src.Technique.Title));

            CreateMap<CreateQuizQuestionDto, QuizQuestion>();
            CreateMap<UpdateQuizQuestionDto, QuizQuestion>();

            CreateMap<AnswerOption, AnswerOptionDto>();
            CreateMap<CreateAnswerOptionDto, AnswerOption>();
        }
    }
}