using Microsoft.AspNetCore.Http;
using TechECommerceServer.Application.Exceptions.Utils;

namespace TechECommerceServer.Application.Exceptions
{
    public class ExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exc)
            {
                await HandleExceptionAsync(context, exc);
            }
        }

        private static Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            int statusCode = GetStatusCode(exception);

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;

            List<string> errors = new List<string>()
            {
                $"Error message: {exception.Message}",
                $"Message description: {exception.InnerException?.ToString()}"
            };

            return httpContext.Response.WriteAsync(new ExceptionModel()
            {
                Errors = errors,
                StatusCode = statusCode
            }.ToString());
        }

        private static int GetStatusCode(Exception exception)
        {
            Type exceptionType = exception.GetType();

            if (StatusCodeDictionary.StatusCodeMappings.TryGetValue(exceptionType, out int statusCode))
            {
                return statusCode;
            }

            // note: if the exception type is not explicitly mapped, return 500 Internal Server Error.
            return StatusCodes.Status500InternalServerError;
        }

    }
}
