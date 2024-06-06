using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TechECommerceServer.Application.Exceptions;

namespace TechECommerceServer.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssemblies(assembly);
            });

            services.AddTransient<ExceptionMiddleware>();
        }
    }
}
