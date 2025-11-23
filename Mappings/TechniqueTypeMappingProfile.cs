using AutoMapper;
using FirstAidAPI.DTO.Technique;
using FirstAidAPI.Models;

namespace FirstAidAPI.Mappings
{
    public class TechniqueTypeMappingProfile : Profile
    {
        public TechniqueTypeMappingProfile()
        {
            CreateMap<TechniqueType, TechniqueTypeDto>();
            CreateMap<CreateTechniqueTypeDto, TechniqueType>();
            CreateMap<UpdateTechniqueTypeDto, TechniqueType>();
        }
    }
}