using MediaBrowser.Common.Security;
using Microsoft.AspNetCore.Http;
using SendGrid.Helpers.Errors.Model;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security;
using System.Security.Authentication;

namespace TechECommerceServer.Application.Exceptions.Utils
{
    public static class StatusCodeDictionary
    {
        public static readonly Dictionary<Type, int> StatusCodeMappings = new Dictionary<Type, int>
        {
            // Validation and Bad Requests
            { typeof(BadRequestException), StatusCodes.Status400BadRequest },
            { typeof(ArgumentNullException), StatusCodes.Status400BadRequest },
            { typeof(ArgumentException), StatusCodes.Status400BadRequest },
            { typeof(FormatException), StatusCodes.Status400BadRequest },
            { typeof(ValidationException), StatusCodes.Status422UnprocessableEntity },

            // Authorization and Authentication
            { typeof(UnauthorizedAccessException), StatusCodes.Status401Unauthorized },
            { typeof(AuthenticationException), StatusCodes.Status401Unauthorized },
            { typeof(SecurityException), StatusCodes.Status403Forbidden },
            { typeof(AccessViolationException), StatusCodes.Status403Forbidden },

            // Resource Not Found
            { typeof(NotFoundException), StatusCodes.Status404NotFound },
            { typeof(KeyNotFoundException), StatusCodes.Status404NotFound },
            { typeof(FileNotFoundException), StatusCodes.Status404NotFound },

            // Conflict and Concurrency
            { typeof(InvalidOperationException), StatusCodes.Status409Conflict },
            { typeof(DuplicateNameException), StatusCodes.Status409Conflict },

            // Server Errors
            { typeof(NotImplementedException), StatusCodes.Status501NotImplemented },

            // File and Upload Errors
            { typeof(FileLoadException), StatusCodes.Status500InternalServerError },
            { typeof(IOException), StatusCodes.Status500InternalServerError },
            { typeof(PathTooLongException), StatusCodes.Status400BadRequest },
            { typeof(UnauthorizedAccessException), StatusCodes.Status401Unauthorized },
        
            // Service and Network Errors
            { typeof(HttpRequestException), StatusCodes.Status503ServiceUnavailable },
            { typeof(TimeoutException), StatusCodes.Status408RequestTimeout },
        
            // Payment and Business Logic Errors
            { typeof(PaymentRequiredException), StatusCodes.Status402PaymentRequired }
        };
    }
}
