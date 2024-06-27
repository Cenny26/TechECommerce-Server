using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TechECommerceServer.Application.Abstractions.Token;
using TechECommerceServer.Application.Abstractions.Token.Utils;
using TechECommerceServer.Application.Bases;
using TechECommerceServer.Application.Features.Commands.AppUser.Exceptions;
using TechECommerceServer.Application.Features.Commands.AppUser.Rules;
using TechECommerceServer.Domain.DTOs.Auth;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace TechECommerceServer.Application.Features.Commands.AppUser.GoogleLogInAppUser
{
    public class GoogleLogInAppUserCommandHandler : BaseHandler, IRequestHandler<GoogleLogInAppUserCommandRequest, GoogleLogInAppUserCommandResponse>
    {
        private readonly ITokenHandler _tokenHandler;
        private readonly IConfiguration _configuration;
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        private readonly BaseUserRules _userRules;
        public GoogleLogInAppUserCommandHandler(IMapper _mapper, ITokenHandler tokenHandler, IConfiguration configuration, UserManager<Domain.Entities.Identity.AppUser> userManager, BaseUserRules userRules) : base(_mapper)
        {
            _tokenHandler = tokenHandler;
            _configuration = configuration;
            _userManager = userManager;
            _userRules = userRules;
        }

        public async Task<GoogleLogInAppUserCommandResponse> Handle(GoogleLogInAppUserCommandRequest request, CancellationToken cancellationToken)
        {
            // note: replacing the validation class with a simple rule instead
            await _userRules.GivenIdTokenMustBeLoadWhenProcessToGoogleLogIn(request.IdToken);

            try
            {
                ValidationSettings validationSettings = new ValidationSettings()
                {
                    Audience = new List<string> { _configuration["ExternalLoginSettings:OAuth:Google:ClientId"] }
                };

                Payload payload = await ValidateAsync(request.IdToken, validationSettings);

                UserLoginInfo userLoginInfo = new UserLoginInfo(request.Provider, payload.Subject, request.Provider);
                Domain.Entities.Identity.AppUser? appUser = await _userManager.FindByLoginAsync(userLoginInfo.LoginProvider, userLoginInfo.ProviderKey);

                bool userResult = appUser is not null;
                if (appUser is null)
                {
                    appUser = await _userManager.FindByEmailAsync(payload.Email);
                    if (appUser is null)
                    {
                        appUser = new Domain.Entities.Identity.AppUser()
                        {
                            Id = Guid.NewGuid().ToString(),
                            FullName = payload.Name,
                            UserName = payload.Email,
                            Email = payload.Email
                        };

                        IdentityResult identityResult = await _userManager.CreateAsync(appUser);
                        userResult = identityResult.Succeeded;
                    }
                }

                if (userResult)
                    await _userManager.AddLoginAsync(appUser, userLoginInfo);

                Token token = _tokenHandler.CreateAccessToken(DefaultTokenVariables.StandardTokenValue); // note: default 60 minute for expire!
                return new GoogleLogInAppUserCommandResponse()
                {
                    Token = token
                };
            }
            catch (Exception exc)
            {
                throw new UserGoogleLoginAuthenticationProcessException();
            }
        }
    }
}
