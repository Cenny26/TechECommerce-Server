using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TechECommerceServer.Persistence.Configurations;
using TechECommerceServer.Persistence.Contexts;

namespace TechECommerceServer.Persistence.DesignTimeDbContextFactories
{
    public class DesignTimeDefaultDbContextFactory : IDesignTimeDbContextFactory<TechECommerceServerDbContext>
    {
        public TechECommerceServerDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<TechECommerceServerDbContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<TechECommerceServerDbContext>();
            dbContextOptionsBuilder.UseNpgsql(DefaultDbConnectionStringConfiguration.ConnectionString);

            return new TechECommerceServerDbContext(dbContextOptionsBuilder.Options);
        }
    }
}
