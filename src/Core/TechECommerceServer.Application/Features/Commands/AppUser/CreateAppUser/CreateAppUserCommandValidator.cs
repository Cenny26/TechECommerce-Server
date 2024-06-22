using FluentValidation;

namespace TechECommerceServer.Application.Features.Commands.AppUser.CreateAppUser
{
    public class CreateAppUserCommandValidator : AbstractValidator<CreateAppUserCommandRequest>
    {
        public CreateAppUserCommandValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Please do not leave the full name blank.")
                .MaximumLength(50).WithMessage("Full name can be up to 50 characters.")
                .WithName("user's full name");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Please do not leave the username blank.")
                .MaximumLength(30).WithMessage("Username can be up to 30 characters.")
                .WithName("user's username");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Please do not leave the email blank.")
                .EmailAddress().WithMessage("Please provide a valid email address.")
                .WithName("user's email");

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
