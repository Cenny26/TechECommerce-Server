using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace TechECommerceServer.API.Filters.Constraints
{
    public class ValidateGuidQueryParameterAttribute : ActionFilterAttribute
    {
        private readonly string _parameterName;
        public ValidateGuidQueryParameterAttribute(string parameterName)
        {
            _parameterName = parameterName;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            IQueryCollection query = context.HttpContext.Request.Query;
            if (query.TryGetValue(_parameterName, out StringValues value) && !Guid.TryParse(value, out _))
                context.Result = new BadRequestObjectResult($"Invalid {_parameterName}");
        }
    }
}
