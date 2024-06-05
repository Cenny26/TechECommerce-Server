using TechECommerceServer.Application.Abstractions.Repositories.Product;
using TechECommerceServer.Persistence.Contexts;

namespace TechECommerceServer.Persistence.Concretes.Repositories.Product
{
    public class ProductWriteRepository : WriteRepository<Domain.Entities.Product>, IProductWriteRepository
    {
        public ProductWriteRepository(TechECommerceServerDbContext commerceServerDbContext) : base(commerceServerDbContext)
        {
        }
    }
}
