using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TechECommerceServer.Domain.Entities;
using TechECommerceServer.Domain.Entities.Common;
using File = TechECommerceServer.Domain.Entities.File;

namespace TechECommerceServer.Persistence.Contexts
{
    public class TechECommerceServerDbContext : DbContext
    {
        protected TechECommerceServerDbContext() { }
        public TechECommerceServerDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<EntityEntry<BaseEntity>> datas = ChangeTracker.Entries<BaseEntity>();
            foreach (var data in datas)
            {
                _ = data.State switch
                {
                    EntityState.Added => data.Entity.CreatedDate = DateTime.UtcNow,
                    EntityState.Modified => data.Entity.ModifiedDate = DateTime.UtcNow
                };
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
