using Microsoft.AspNetCore.Http;
using SendGrid.Helpers.Errors.Model;
using System.ComponentModel.DataAnnotations;

namespace TechECommerceServer.Application.Exceptions.Utils
{
    public static class StatusCodeDictionary
    {
        public static readonly Dictionary<Type, int> StatusCodeMappings = new Dictionary<Type, int>
        {
            { typeof(BadRequestException), StatusCodes.Status400BadRequest },
            { typeof(NotFoundException), StatusCodes.Status404NotFound },
            { typeof(ValidationException), StatusCodes.Status422UnprocessableEntity },
            { typeof(UnauthorizedAccessException), StatusCodes.Status401Unauthorized },
            { typeof(InvalidOperationException), StatusCodes.Status409Conflict },
            { typeof(NotImplementedException), StatusCodes.Status501NotImplemented }
        };
    }
}
