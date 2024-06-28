using Microsoft.AspNetCore.Identity;
using TechECommerceServer.Application.Abstractions.Services.AppUser;
using TechECommerceServer.Application.Features.Commands.AppUser.Rules;
using TechECommerceServer.Domain.DTOs.AppUser;

namespace TechECommerceServer.Persistence.Concretes.Services.AppUser
{
    public class AppUserService : IAppUserService
    {
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        private readonly BaseUserRules _userRules;
        public AppUserService(UserManager<Domain.Entities.Identity.AppUser> userManager, BaseUserRules userRules)
        {
            _userManager = userManager;
            _userRules = userRules;
        }

        public async Task<CreateAppUserResponseDto> CreateAppUserAsync(CreateAppUserRequestDto model)
        {
            // note: all rules in services should be handled in the Application layer!
            await _userRules.UserEmailShouldBeUnique(appUser: await _userManager.FindByEmailAsync(model.Email), email: model.Email);

            try
            {
                // todo: need to use AutoMapper for bind automatic!
                Guid newUserId = Guid.NewGuid();
                IdentityResult identityResult = await _userManager.CreateAsync(new Domain.Entities.Identity.AppUser()
                {
                    Id = newUserId.ToString(),
                    FullName = model.FullName,
                    UserName = model.UserName,
                    Email = model.Email,
                    EmailConfirmed = model.Email is not null
                }, model.Password);

                CreateAppUserResponseDto response = new CreateAppUserResponseDto()
                {
                    IsSucceed = identityResult.Succeeded
                };

                if (identityResult.Succeeded)
                    response.Message = $"The new user with id: {newUserId} was successfully created!";
                else
                    foreach (IdentityError error in identityResult.Errors)
                        response.Message = $"{error.Code} - {error.Description}\n";

                return response;
            }
            catch (Exception exc)
            {
                throw new OperationCanceledException("An unexpected error was encountered while creating the user!", exc);
            }
        }
    }
}
