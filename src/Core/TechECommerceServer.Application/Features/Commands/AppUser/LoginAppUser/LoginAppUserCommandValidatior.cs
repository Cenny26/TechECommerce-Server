﻿using FluentValidation;

namespace TechECommerceServer.Application.Features.Commands.AppUser.LoginAppUser
{
    public class LoginAppUserCommandValidatior : AbstractValidator<LoginAppUserCommandRequest>
    {
        public LoginAppUserCommandValidatior()
        {
            RuleFor(x => x.UserNameOrEmail)
                .NotEmpty().WithMessage("Please do not leave the username|email blank.")
                .WithName("user's unique username or email address");

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
