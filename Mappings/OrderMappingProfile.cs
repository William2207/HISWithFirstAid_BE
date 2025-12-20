using AutoMapper;
using System.Xml.Linq;
using FirstAidAPI.Models;
using FirstAidAPI.DTO.Order;

namespace FirstAidAPI.Mappings
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            // Order -> OrderDto
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.PaymentMethod,
                    opt => opt.MapFrom(src => src.PaymentMethod.ToString()))
                .ForMember(dest => dest.PaymentStatus,
                    opt => opt.MapFrom(src => src.PaymentStatus.ToString()))
                .ForMember(dest => dest.OrderStatus,
                    opt => opt.MapFrom(src => src.OrderStatus.ToString()))
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User!.FullName))
                .ForMember(dest => dest.UserEmail,
                    opt => opt.MapFrom(src => src.User!.Email));

            // OrderItem -> OrderItemDto
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.CourseName,
                    opt => opt.MapFrom(src => src.PracticalCourse!.Title));

            // Order -> OrderListDto
            CreateMap<Order, OrderListDto>()
                .ForMember(dest => dest.PaymentStatus,
                    opt => opt.MapFrom(src => src.PaymentStatus.ToString()))
                .ForMember(dest => dest.OrderStatus,
                    opt => opt.MapFrom(src => src.OrderStatus.ToString()))
                .ForMember(dest => dest.TotalItems,
                    opt => opt.MapFrom(src => src.OrderItems.Count));

            CreateMap<CreateOrderDto, Order>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Tự set = DateTime.Now
                .ForMember(dest => dest.User, opt => opt.Ignore()) // Navigation property
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Items));

            CreateMap<CreateOrderItemDto, OrderItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // ID tự tăng
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.OrderId, opt => opt.Ignore());
        }
    }
}