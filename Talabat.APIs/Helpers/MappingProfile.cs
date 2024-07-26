using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order;
using OrderAddress  =  Talabat.Core.Entities.Order.Address;
using UserAddress   =  Talabat.Core.Entities.Identity.Address;

namespace Talabat.APIs.Helpers
{
    public class MappingProfile : Profile
	{
        public MappingProfile()
        {

            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl,o => o.MapFrom<ProductPictureUrlResolver>()) ;




			CreateMap<UserAddress, AddressDto>().ReverseMap();

			//CreateMap<OrderAddress, AddressDto>().ReverseMap();

			CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();

			CreateMap<BasketItem, BasketItemDto>().ReverseMap();



			CreateMap<Order, OrderToReturnDto>()
				 .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
				 .ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));




			CreateMap<OrderItem, OrderItemDto>()
				 .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
				 .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
				 .ForMember(d => d.PicturetUrl, o => o.MapFrom(s => s.Product.PicturetUrl))
				 .ForMember(d => d.PicturetUrl, o => o.MapFrom<OrderItemPictureUrlResolver>());




			CreateMap<OrderAddress, AddressDto>()
				.ForMember(d => d.FirstName, o => o.MapFrom(s => s.FName))
			    .ForMember(d => d.LastName, o => o.MapFrom(s => s.LName))
			    .ReverseMap();


		}



	}
}
