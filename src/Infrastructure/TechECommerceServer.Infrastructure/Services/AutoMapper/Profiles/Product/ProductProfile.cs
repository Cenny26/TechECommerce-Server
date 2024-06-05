using AutoMapper;
using TechECommerceServer.Application.Features.Commands.Product.CreateProduct;

namespace TechECommerceServer.Infrastructure.Services.AutoMapper.Profiles.Product
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductCommandRequest, Domain.Entities.Product>().ReverseMap();
        }
    }
}
