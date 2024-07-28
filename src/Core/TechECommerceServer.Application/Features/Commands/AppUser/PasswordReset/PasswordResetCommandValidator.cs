using FluentValidation;

namespace TechECommerceServer.Application.Features.Commands.AppUser.PasswordReset
{
    public class PasswordResetCommandValidator : AbstractValidator<PasswordResetCommandRequest>
    {
        public PasswordResetCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Please do not leave the email blank.")
                .EmailAddress().WithMessage("Please provide a valid email address.")
                .WithName("user's email");
        }
    }
}
