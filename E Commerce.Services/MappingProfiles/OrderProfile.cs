using AutoMapper;
using E_Commerce.Domain.Entities.OrderModule;
using E_Commerce.Shared.DTOs.OrderDTOs;

namespace E_Commerce.Services.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<AddressDTO, OrderAddress>().ReverseMap();

            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(D => D.DeliveryMethod, O => O.MapFrom(src => src.DeliveryMethod.ShortName));

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(D => D.ProductName, o => o.MapFrom(S => S.Product.ProductName))
                .ForMember(D => D.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolver>());
        }
    }
}
