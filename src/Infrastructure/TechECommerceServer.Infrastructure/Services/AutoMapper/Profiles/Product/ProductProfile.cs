using AutoMapper;
using TechECommerceServer.Application.Features.Commands.Product.CreateProduct;
using TechECommerceServer.Application.Features.Commands.Product.UpdateProduct;
using TechECommerceServer.Application.Features.Queries.Product.GetAllProducts;
using TechECommerceServer.Application.Features.Queries.Product.GetProductById;
using TechECommerceServer.Application.Helpers.Product;
using TechECommerceServer.Domain.DTOs.Products;

namespace TechECommerceServer.Infrastructure.Services.AutoMapper.Profiles.Product
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductCommandRequest, Domain.Entities.Product>().ReverseMap();
            CreateMap<Domain.Entities.Product, GetAllProductsQueryResponse>()
                .ForMember(destinationMember => destinationMember.DiscountedPrice, memberOption => memberOption.MapFrom(src => ProductPrice.CalculateDiscountedPrice(src.Price, src.Discount)));
            CreateMap<Domain.Entities.Product, ProductsByPagingDto>()
                .ForMember(destinationMember => destinationMember.DiscountedPrice, memberOptions => memberOptions.MapFrom(src => ProductPrice.CalculateDiscountedPrice(src.Price, src.Discount)));
            CreateMap<Domain.Entities.Product, GetProductByIdQueryResponse>()
                .ForMember(destinationMember => destinationMember.DiscountedPrice, memberOptions => memberOptions.MapFrom(src => ProductPrice.CalculateDiscountedPrice(src.Price, src.Discount)));
            CreateMap<UpdateProductCommandRequest, Domain.Entities.Product>().ReverseMap();
        }
    }
}
