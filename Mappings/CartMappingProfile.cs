using AutoMapper;
using FirstAidAPI.DTO.Cart;
using FirstAidAPI.Models;

namespace FirstAidAPI.Mappings
{
    public class CartMappingProfile : Profile
    {
        public CartMappingProfile()
        {
            // Cart -> CartResponseDto
            CreateMap<Cart, CartResponseDto>()
               .ForMember(dest => dest.TotalAmount,
                   opt => opt.MapFrom(src => src.CartItems.Sum(item => item.Price * item.Quantity)))
               .ForMember(dest => dest.TotalItems,
                   opt => opt.MapFrom(src => src.CartItems.Sum(item => item.Quantity)));

            // CartItem -> CartItemResponseDto
            CreateMap<CartItem, CartItemResponseDto>()
                .ForMember(dest => dest.CourseName,
                    opt => opt.MapFrom(src => src.PracticalCourse != null ? src.PracticalCourse.Title : string.Empty))
                .ForMember(dest => dest.CourseImage,
                    opt => opt.MapFrom(src => src.PracticalCourse != null ? src.PracticalCourse.Icon : null))
                .ForMember(dest => dest.Subtotal,
                    opt => opt.MapFrom(src => src.Price * src.Quantity));

            // AddCartItemRequestDto -> CartItem (for creating new item)
            CreateMap<AddCartItemRequestDto, CartItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CartId, opt => opt.Ignore())
                .ForMember(dest => dest.Price, opt => opt.Ignore()) // Set from course price
                .ForMember(dest => dest.AddedAt, opt => opt.MapFrom(src => DateOnly.FromDateTime(DateTime.UtcNow)))
                .ForMember(dest => dest.Cart, opt => opt.Ignore())
                .ForMember(dest => dest.PracticalCourse, opt => opt.Ignore());
        }
    }
}