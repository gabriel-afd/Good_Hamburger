using AutoMapper;
using GoodHamburger.Application.DTOs.Response;
using GoodHamburger.Domain.Entities;


namespace GoodHamburger.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MenuItem, MenuItemResponse>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));

            CreateMap<OrderItem, OrderItemResponse>()
                .ForMember(dest => dest.MenuItemName, opt => opt.MapFrom(src => src.MenuItem.Name))
                .ForMember(dest => dest.MenuItemPrice, opt => opt.MapFrom(src => src.MenuItem.Price));

            CreateMap<Order, OrderResponse>()
                .ForMember(dest => dest.Subtotal, opt => opt.MapFrom(src => src.GetSubtotal()))
                .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.GetDiscount()))
                .ForMember(dest => dest.DiscountPercentage, opt => opt.MapFrom(src => src.DiscountPercentage))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.GetTotal()));
        }
    }
}
