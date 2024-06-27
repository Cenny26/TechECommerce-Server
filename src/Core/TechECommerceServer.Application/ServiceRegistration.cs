using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Reflection;
using TechECommerceServer.Application.Bases;
using TechECommerceServer.Application.Behaviors.Cache;
using TechECommerceServer.Application.Behaviors.Validation;
using TechECommerceServer.Application.Exceptions;

namespace TechECommerceServer.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Configure MediatR
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssemblies(assembly);
            });

            // Configure HttpClient and HttpClientFactory
            services.AddHttpClient();

            // Register custom rules and validators
            services.AddRulesFromAssemblyContaining(assembly, typeof(BaseRule));
            services.AddValidatorsFromAssembly(assembly);
            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en-US");

            // Add custom middleware
            services.AddTransient<ExceptionMiddleware>();

            // Add custom behaviors
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FluentValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RedisCacheBehavior<,>));

            // Configure API behavior options
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }

        private static IServiceCollection AddRulesFromAssemblyContaining(this IServiceCollection services, Assembly assembly, Type type)
        {
            List<Type> types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();
            foreach (var item in types)
                services.AddTransient(item);

            return services;
        }
    }
}
