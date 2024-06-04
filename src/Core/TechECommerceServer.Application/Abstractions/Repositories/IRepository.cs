using Microsoft.EntityFrameworkCore;
using TechECommerceServer.Domain.Entities.Common;

namespace TechECommerceServer.Application.Abstractions.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        public DbSet<T> Table { get; }
    }
}
