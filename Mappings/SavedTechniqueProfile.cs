using AutoMapper;
using FirstAidAPI.DTO.SavedTechniques;
using FirstAidAPI.Models;

namespace FirstAidAPI.Mappings
{
    public class SavedTechniqueProfile : Profile
    {
        public SavedTechniqueProfile()
        {
            // Từ Entity -> DTO (khi trả về cho client)
            CreateMap<SavedTechnique, SavedTechniqueNewDto>()
                .ForMember(dest => dest.TechniqueName, opt => opt.MapFrom(src => src.Technique.Name))
                .ForMember(dest => dest.TechniqueTitle, opt => opt.MapFrom(src => src.Technique.Title))
                .ForMember(dest => dest.TechniqueDifficulty, opt => opt.MapFrom(src => src.Technique.Difficulty));

            // Từ AddDto -> Entity (khi tạo mới)
            CreateMap<AddSavedTechniqueDto, SavedTechnique>()
                .ForMember(dest => dest.SavedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Technique, opt => opt.Ignore());
        }
    }
}