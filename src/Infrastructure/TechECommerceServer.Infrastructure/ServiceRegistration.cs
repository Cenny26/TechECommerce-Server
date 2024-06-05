using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TechECommerceServer.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            services.AddAutoMapper(assembly);
        }
    }
}
