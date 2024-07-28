using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TechECommerceServer.Application.Abstractions.Services.AppUser;
using TechECommerceServer.Application.Features.Commands.AppUser.Exceptions;
using TechECommerceServer.Application.Features.Commands.AppUser.Rules;
using TechECommerceServer.Application.Helpers.UrlCoding;
using TechECommerceServer.Domain.DTOs.AppUser;
using TechECommerceServer.Domain.DTOs.AppUser.UpdatePassword;

namespace TechECommerceServer.Persistence.Concretes.Services.AppUser
{
    public class AppUserService : IAppUserService
    {
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        private readonly BaseUserRules _userRules;
        private readonly ILogger<AppUserService> _logger;
        public AppUserService(UserManager<Domain.Entities.Identity.AppUser> userManager, BaseUserRules userRules, ILogger<AppUserService> logger)
        {
            _userManager = userManager;
            _userRules = userRules;
            _logger = logger;
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

        public async Task UpdateAppUserPasswordAsync(UpdatePasswordRequestDto model)
        {
            _logger.LogDebug("Entering {MethodName} with user ID: {UserId}", nameof(UpdateAppUserPasswordAsync), model.UserId);

            Domain.Entities.Identity.AppUser? appUser = await _userManager.FindByIdAsync(model.UserId);

            try
            {
                if (appUser is not null)
                {
                    model.ResetToken = model.ResetToken.UrlDecode();

                    IdentityResult identityResult = await _userManager.ResetPasswordAsync(appUser, model.ResetToken, model.Password);
                    if (identityResult.Succeeded)
                        await _userManager.UpdateSecurityStampAsync(appUser);
                    else throw new Exception("An unexpected error was encountered while updating the password!");
                }
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, "An error occurred while updating the password!");
                throw new Exception("An error occurred while updating the password!", exc);
            }
            finally
            {
                _logger.LogDebug("Method {MethodName} ended.", nameof(UpdateAppUserPasswordAsync));
            }
        }

        public async Task UpdateRefreshToken(string refreshToken, Domain.Entities.Identity.AppUser appUser, DateTime accessTokenDate, int addOnAccessTokenDate)
        {
            if (appUser is not null)
            {
                appUser.RefreshToken = refreshToken;
                appUser.RefreshTokenEndDate = accessTokenDate.AddSeconds(addOnAccessTokenDate);

                await _userManager.UpdateAsync(appUser);
            }
            else
                throw new NotUserFoundedException();
        }
    }
}
