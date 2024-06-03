using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechECommerceServer.Persistence.Configurations;
using TechECommerceServer.Persistence.Contexts;

namespace TechECommerceServer.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<TechECommerceServerDbContext>(options =>
            {
                options.UseNpgsql(DefaultDbConnectionStringConfiguration.ConnectionString);
            });
        }
    }
}
