using TechECommerceServer.Application.Abstractions.Repositories.File;
using TechECommerceServer.Persistence.Contexts;

namespace TechECommerceServer.Persistence.Concretes.Repositories.File
{
    public class FileWriteRepository : WriteRepository<Domain.Entities.File>, IFileWriteRepository
    {
        public FileWriteRepository(TechECommerceServerDbContext commerceServerDbContext) : base(commerceServerDbContext)
        {
        }
    }
}
