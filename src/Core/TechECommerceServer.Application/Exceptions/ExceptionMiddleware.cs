using FluentValidation;
using MediaBrowser.Common.Security;
using Microsoft.AspNetCore.Http;
using SendGrid.Helpers.Errors.Model;
using System.Data;
using System.Security;
using System.Security.Authentication;

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

            if (exception.GetType() == typeof(ValidationException))
                return httpContext.Response.WriteAsync(new ExceptionModel()
                {
                    Errors = ((ValidationException)exception).Errors.Select(element => element.ErrorMessage),
                    StatusCode = StatusCodes.Status422UnprocessableEntity
                }.ToString());

            List<string> errors = new List<string>()
            {
                $"Error message: {exception.Message}"
            };

            return httpContext.Response.WriteAsync(new ExceptionModel()
            {
                Errors = errors,
                StatusCode = statusCode
            }.ToString());
        }

        private static int GetStatusCode(Exception exception) =>
            exception switch
            {
                // Validation and Bad Requests
                BadRequestException => StatusCodes.Status400BadRequest,
                ArgumentNullException => StatusCodes.Status400BadRequest,
                ArgumentException => StatusCodes.Status400BadRequest,
                FormatException => StatusCodes.Status400BadRequest,
                ValidationException => StatusCodes.Status422UnprocessableEntity,
                PathTooLongException => StatusCodes.Status400BadRequest,

                // Authorization and Authentication
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                AuthenticationException => StatusCodes.Status401Unauthorized,
                SecurityException => StatusCodes.Status403Forbidden,
                AccessViolationException => StatusCodes.Status403Forbidden,

                // Resource Not Found
                NotFoundException => StatusCodes.Status404NotFound,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                FileNotFoundException => StatusCodes.Status404NotFound,

                // Conflict and Concurrency
                InvalidOperationException => StatusCodes.Status409Conflict,
                DuplicateNameException => StatusCodes.Status409Conflict,

                // Server Errors
                NotImplementedException => StatusCodes.Status501NotImplemented,

                // File and Upload Errors
                FileLoadException => StatusCodes.Status500InternalServerError,
                IOException => StatusCodes.Status500InternalServerError,

                // Service and Network Errors
                HttpRequestException => StatusCodes.Status503ServiceUnavailable,
                TimeoutException => StatusCodes.Status408RequestTimeout,

                // Payment and Business Logic Errors
                PaymentRequiredException => StatusCodes.Status402PaymentRequired,

                // Default to Internal Server Error for any other exceptions
                _ => StatusCodes.Status500InternalServerError
            };
    }
}
