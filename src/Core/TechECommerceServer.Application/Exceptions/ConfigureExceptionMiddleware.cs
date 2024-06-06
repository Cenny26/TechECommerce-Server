using Microsoft.AspNetCore.Builder;

namespace TechECommerceServer.Application.Exceptions
{
    public static class ConfigureExceptionMiddleware
    {
        public static void ConfigureExceptionHandlingMiddleware(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
