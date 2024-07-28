using FluentValidation;
using TechECommerceServer.Application.Extensions.Validators;

namespace TechECommerceServer.Application.Features.Commands.AppUser.UpdatePassword
{
    public class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommandRequest>
    {
        public UpdatePasswordCommandValidator()
        {
            RuleFor(x => x.ResetToken)
                .NotEmpty().WithMessage("ResetToken is required.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User identity is required.")
                .Must(GuidTypeValidators.IsValidGuid).WithMessage("App User identity must be a valid GUID.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Please do not leave the password blank.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .WithName("user's password");

            RuleFor(x => x.PasswordConfirm)
                .NotEmpty().WithMessage("Please do not leave the password confirmation blank.")
                .Equal(x => x.Password).WithMessage("Passwords do not match.")
                .WithName("password confirmation");
        }
    }
}
