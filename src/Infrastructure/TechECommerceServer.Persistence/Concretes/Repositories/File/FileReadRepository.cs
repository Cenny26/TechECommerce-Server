using TechECommerceServer.Application.Abstractions.Repositories.File;
using TechECommerceServer.Persistence.Contexts;

namespace TechECommerceServer.Persistence.Concretes.Repositories.File
{
    public class FileReadRepository : ReadRepository<Domain.Entities.File>, IFileReadRepository
    {
        public FileReadRepository(TechECommerceServerDbContext commerceServerDbContext) : base(commerceServerDbContext)
        {
        }
    }
}
