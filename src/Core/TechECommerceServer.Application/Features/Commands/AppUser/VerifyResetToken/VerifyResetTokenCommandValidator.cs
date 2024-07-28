using FluentValidation;
using TechECommerceServer.Application.Extensions.Validators;

namespace TechECommerceServer.Application.Features.Commands.AppUser.VerifyResetToken
{
    public class VerifyResetTokenCommandValidator : AbstractValidator<VerifyResetTokenCommandRequest>
    {
        public VerifyResetTokenCommandValidator()
        {
            RuleFor(x => x.ResetToken)
                .NotEmpty().WithMessage("ResetToken is required.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User identity is required.")
                .Must(GuidTypeValidators.IsValidGuid).WithMessage("App User identity must be a valid GUID.");
        }
    }
}
