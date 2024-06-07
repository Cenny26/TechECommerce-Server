using TechECommerceServer.Application.Abstractions.Repositories.ProductImage;
using TechECommerceServer.Persistence.Contexts;

namespace TechECommerceServer.Persistence.Concretes.Repositories.ProductImage
{
    public class ProductImageWriteRepository : WriteRepository<Domain.Entities.ProductImage>, IProductImageWriteRepository
    {
        public ProductImageWriteRepository(TechECommerceServerDbContext commerceServerDbContext) : base(commerceServerDbContext)
        {
        }
    }
}
