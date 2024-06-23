using TechECommerceServer.Application.Bases;
using TechECommerceServer.Application.Features.Commands.AppUser.Exceptions;

namespace TechECommerceServer.Application.Features.Commands.AppUser.Rules
{
    public class BaseUserRules : BaseRule
    {
        public Task UserEmailShouldBeUnique(Domain.Entities.Identity.AppUser? appUser, string email)
        {
            if (appUser is not null)
                throw new UserAlreadyExistsException(email);

            return Task.CompletedTask;
        }

        public Task GivenAppUserMustBeLoadWhenProcessToLogIn(Domain.Entities.Identity.AppUser appUser)
        {
            if (appUser is null)
                throw new NotUserFoundedException();

            return Task.CompletedTask;
        }
    }
}
