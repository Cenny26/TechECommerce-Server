using AutoMapper;
using TechECommerceServer.Application.Features.Commands.Product.CreateProduct;
using TechECommerceServer.Application.Features.Queries.Product.GetAllProducts;
using TechECommerceServer.Application.Helpers.Product;

namespace TechECommerceServer.Infrastructure.Services.AutoMapper.Profiles.Product
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductCommandRequest, Domain.Entities.Product>().ReverseMap();
            CreateMap<Domain.Entities.Product, GetAllProductsQueryResponse>()
                .ForMember(destinationMember => destinationMember.DiscountedPrice, memberOption => memberOption.MapFrom(src => ProductPrice.CalculateDiscountedPrice(src.Price, src.Discount)));
        }
    }
}
