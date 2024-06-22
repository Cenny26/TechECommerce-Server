using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TechECommerceServer.Domain.Entities;
using TechECommerceServer.Domain.Entities.Common;
using TechECommerceServer.Domain.Entities.Identity;
using File = TechECommerceServer.Domain.Entities.File;

namespace TechECommerceServer.Persistence.Contexts
{
    public class TechECommerceServerDbContext : IdentityDbContext<AppUser, AppRole, string>
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
                    EntityState.Modified => data.Entity.ModifiedDate = DateTime.UtcNow,
                    _ => DateTime.UtcNow
                };
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
