using System.ComponentModel.DataAnnotations;

namespace TechECommerceServer.Application.Filters.Attributes
{
    public class GuidValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string strValue && !Guid.TryParse(strValue, out _))
                return new ValidationResult("Invalid GUID format");

            return ValidationResult.Success;
        }
    }
}
