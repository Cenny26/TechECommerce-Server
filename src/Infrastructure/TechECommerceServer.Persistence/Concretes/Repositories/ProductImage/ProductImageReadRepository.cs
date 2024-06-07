using TechECommerceServer.Application.Abstractions.Repositories.ProductImage;
using TechECommerceServer.Persistence.Contexts;

namespace TechECommerceServer.Persistence.Concretes.Repositories.ProductImage
{
    public class ProductImageReadRepository : ReadRepository<Domain.Entities.ProductImage>, IProductImageReadRepository
    {
        public ProductImageReadRepository(TechECommerceServerDbContext commerceServerDbContext) : base(commerceServerDbContext)
        {
        }
    }
}
