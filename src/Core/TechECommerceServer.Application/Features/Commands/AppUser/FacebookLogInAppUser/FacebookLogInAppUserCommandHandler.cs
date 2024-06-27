using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using TechECommerceServer.Application.Abstractions.Token;
using TechECommerceServer.Application.Abstractions.Token.Utils;
using TechECommerceServer.Application.Bases;
using TechECommerceServer.Application.Features.Commands.AppUser.Exceptions;
using TechECommerceServer.Application.Features.Commands.AppUser.Rules;
using TechECommerceServer.Domain.DTOs.Auth;
using TechECommerceServer.Domain.DTOs.Auth.Facebook;

namespace TechECommerceServer.Application.Features.Commands.AppUser.FacebookLogInAppUser
{
    public class FacebookLogInAppUserCommandHandler : BaseHandler, IRequestHandler<FacebookLogInAppUserCommandRequest, FacebookLogInAppUserCommandResponse>
    {
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        private readonly ITokenHandler _tokenHandler;
        private readonly IConfiguration _configuration;
        private readonly BaseUserRules _userRules;
        private readonly HttpClient _httpClient;
        public FacebookLogInAppUserCommandHandler(IMapper _mapper, UserManager<Domain.Entities.Identity.AppUser> userManager, ITokenHandler tokenHandler, System.Net.Http.IHttpClientFactory httpClientFactory, IConfiguration configuration, BaseUserRules userRules) : base(_mapper)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _configuration = configuration;
            _userRules = userRules;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<FacebookLogInAppUserCommandResponse> Handle(FacebookLogInAppUserCommandRequest request, CancellationToken cancellationToken)
        {
            string provider = _configuration["ExternalLoginSettings:OAuth:Facebook:Provider"]; // note: 'FACEBOOK'

            // note: replacing the validation class with a simple rule instead
            await _userRules.GivenAuthTokenMustBeLoadWhenProcessToFacebookLogIn(request.AuthToken);

            try
            {
                string accessTokenUrl = $"https://graph.facebook.com/oauth/access_token?client_id={_configuration["ExternalLoginSettings:OAuth:Facebook:ClientId"]}&client_secret={_configuration["ExternalLoginSettings:OAuth:Facebook:ClientSecret"]}&grant_type=client_credentials";
                string accessTokenResponse = await _httpClient.GetStringAsync(accessTokenUrl);

                FacebookAccessTokenResponseDto facebookAccessTokenResponse = JsonSerializer.Deserialize<FacebookAccessTokenResponseDto>(accessTokenResponse);

                string userAccessTokenValidationUrl = $"https://graph.facebook.com/debug_token?input_token={request.AuthToken}&access_token={facebookAccessTokenResponse.AccessToken}";
                string userAccessTokenValidationResponse = await _httpClient.GetStringAsync(userAccessTokenValidationUrl);

                FacebookUserAccessTokenValidationResponseDto facebookUserAccessTokenValidationResponse = JsonSerializer.Deserialize<FacebookUserAccessTokenValidationResponseDto>(userAccessTokenValidationResponse);

                if (facebookUserAccessTokenValidationResponse.Data.IsValid)
                {
                    string userInfoUrl = $"https://graph.facebook.com/me?fields=email,name&access_token={request.AuthToken}";
                    string userInfoResponse = await _httpClient.GetStringAsync(userInfoUrl);

                    FacebookUserInfoResponseDto facebookUserInfoResponse = JsonSerializer.Deserialize<FacebookUserInfoResponseDto>(userInfoResponse);

                    UserLoginInfo userLoginInfo = new UserLoginInfo(provider, facebookUserAccessTokenValidationResponse.Data.UserId, provider);
                    Domain.Entities.Identity.AppUser? appUser = await _userManager.FindByLoginAsync(userLoginInfo.LoginProvider, userLoginInfo.ProviderKey);

                    bool userResult = appUser is not null;
                    if (appUser is null)
                    {
                        appUser = await _userManager.FindByEmailAsync(facebookUserInfoResponse.Email);
                        if (appUser is null)
                        {
                            appUser = new Domain.Entities.Identity.AppUser()
                            {
                                Id = Guid.NewGuid().ToString(),
                                FullName = facebookUserInfoResponse.Name,
                                UserName = facebookUserInfoResponse.Email,
                                Email = facebookUserInfoResponse.Email
                            };

                            IdentityResult identityResult = await _userManager.CreateAsync(appUser);
                            userResult = identityResult.Succeeded;
                        }
                    }

                    if (userResult)
                        await _userManager.AddLoginAsync(appUser, userLoginInfo);

                    Token token = _tokenHandler.CreateAccessToken(DefaultTokenVariables.StandardTokenValue); // note: default 60 minute for expire!
                    return new FacebookLogInAppUserCommandResponse()
                    {
                        Token = token
                    };
                }

                else
                    throw new UserFacebookLoginAuthenticationProcessException("User access token values are not validate from external (Facebook) authentication process!");
            }
            catch (Exception exc)
            {
                throw new UserFacebookLoginAuthenticationProcessException("An unexpected error was encountered during the external (Facebook) authentication process!");
            }
        }
    }
}
