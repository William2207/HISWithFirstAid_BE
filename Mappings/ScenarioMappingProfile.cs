using AutoMapper;
using FirstAidAPI.DTO;
using FirstAidAPI.Models;

namespace FirstAidAPI.Mappings
{
    public class ScenarioMappingProfile : Profile
    {
        public ScenarioMappingProfile()
        {
            // Scenario mappings
            CreateMap<Scenario, ScenarioDto>();

            CreateMap<Scenario, ScenarioDetailDto>();

            CreateMap<CreateScenarioDto, Scenario>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ScenarioTechniques, opt => opt.MapFrom(src => src.ScenarioTechniques))
                .ForMember(dest => dest.ScenarioSteps, opt => opt.MapFrom(src => src.ScenarioSteps));

            CreateMap<UpdateScenarioDto, Scenario>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ScenarioTechniques, opt => opt.MapFrom(src => src.ScenarioTechniques))
                .ForMember(dest => dest.ScenarioSteps, opt => opt.MapFrom(src => src.ScenarioSteps));

            // ScenarioTechnique mappings
            CreateMap<ScenarioTechnique, ScenarioTechniqueDto>()
                .ForMember(dest => dest.Technique, opt => opt.MapFrom(src => src.Technique));

            CreateMap<CreateScenarioTechniqueDto, ScenarioTechnique>()
                .ForMember(dest => dest.ScenarioId, opt => opt.Ignore())
                .ForMember(dest => dest.Scenario, opt => opt.Ignore())
                .ForMember(dest => dest.Technique, opt => opt.Ignore());

            // Technique mapping (nếu cần)
            CreateMap<Technique, TechniqueDto>();

            // ScenarioStep mappings
            CreateMap<ScenarioStep, ScenarioStepDto>()
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options));

            CreateMap<CreateScenarioStepDto, ScenarioStep>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ScenarioId, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options))
                .ForMember(dest => dest.Scenario, opt => opt.Ignore())
                .ForMember(dest => dest.Technique, opt => opt.Ignore());

            // StepOption mappings
            CreateMap<StepOption, StepOptionDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OptionKey, opt => opt.MapFrom(src => src.OptionKey))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));
            // Không map IsCorrect và các thông tin nhạy cảm khác

            CreateMap<CreateStepOptionDto, StepOption>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.StepId, opt => opt.Ignore())
                .ForMember(dest => dest.Step, opt => opt.Ignore());
        }
    }
}