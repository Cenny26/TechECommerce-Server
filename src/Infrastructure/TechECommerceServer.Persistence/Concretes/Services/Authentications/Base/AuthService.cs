using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using TechECommerceServer.Application.Abstractions.Services.Authentications.Base;
using TechECommerceServer.Application.Abstractions.Token;
using TechECommerceServer.Application.Features.Commands.AppUser.Exceptions;
using TechECommerceServer.Application.Features.Commands.AppUser.Rules;
using TechECommerceServer.Domain.DTOs.Auth;
using TechECommerceServer.Domain.DTOs.Auth.Facebook;

namespace TechECommerceServer.Persistence.Concretes.Services.Authentications.Base
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly BaseUserRules _userRules;
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        private readonly ITokenHandler _tokenHandler;
        private readonly HttpClient _httpClient;
        public AuthService(IConfiguration configuration, BaseUserRules userRules, IHttpClientFactory httpClientFactory, UserManager<Domain.Entities.Identity.AppUser> userManager, ITokenHandler tokenHandler)
        {
            _configuration = configuration;
            _userRules = userRules;
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<FacebookLogInAppUserResponseDto> FacebookLogInAppUserAsync(FacebookLogInAppUserRequestDto model, int accessTokenLifeTime)
        {
            string provider = _configuration["ExternalLoginSettings:OAuth:Facebook:Provider"] ?? "FACEBOOK"; // note: 'FACEBOOK'

            // note: replacing the validation class with a simple rule instead
            await _userRules.GivenAuthTokenMustBeLoadWhenProcessToFacebookLogIn(model.AuthToken);

            try
            {
                #region Meta for Developers
                // note: let search on: 'https://developers.facebook.com/docs/facebook-login/guides' for more information
                #endregion

                // note: curl -X GET "https://graph.facebook.com/oauth/access_token?client_id={your-app-id}&client_secret={your-app-secret}&grant_type=client_credentials"
                string accessTokenUrl = $"https://graph.facebook.com/oauth/access_token?client_id={_configuration["ExternalLoginSettings:OAuth:Facebook:ClientId"]}&client_secret={_configuration["ExternalLoginSettings:OAuth:Facebook:ClientSecret"]}&grant_type={_configuration["ExternalLoginSettings:OAuth:Facebook:GrantType"]}";
                string accessTokenResponse = await _httpClient.GetStringAsync(accessTokenUrl);

                FacebookAccessTokenResponseDto? facebookAccessTokenResponse = JsonSerializer.Deserialize<FacebookAccessTokenResponseDto>(accessTokenResponse);

                // note: curl -i -X GET "https://graph.facebook.com/debug_token?input_token={input-token}&access_token={valid-access-token}
                string userAccessTokenValidationUrl = $"https://graph.facebook.com/debug_token?input_token={model.AuthToken}&access_token={facebookAccessTokenResponse?.AccessToken}";
                string userAccessTokenValidationResponse = await _httpClient.GetStringAsync(userAccessTokenValidationUrl);

                FacebookUserAccessTokenValidationResponseDto? facebookUserAccessTokenValidationResponse = JsonSerializer.Deserialize<FacebookUserAccessTokenValidationResponseDto>(userAccessTokenValidationResponse);

                if (facebookUserAccessTokenValidationResponse?.Data.IsValid is not null)
                {
                    // note: curl -i -X GET "https://graph.facebook.com/USER-ID?fields=id,name,email,picture&access_token=ACCESS-TOKEN"
                    string userInfoUrl = $"https://graph.facebook.com/me?fields=email,name&access_token={model.AuthToken}";
                    string userInfoResponse = await _httpClient.GetStringAsync(userInfoUrl);

                    FacebookUserInfoResponseDto? facebookUserInfoResponse = JsonSerializer.Deserialize<FacebookUserInfoResponseDto>(userInfoResponse);

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

                    Token token = _tokenHandler.CreateAccessToken(seconds: accessTokenLifeTime); // note: default 60 minute for expire!
                    return new FacebookLogInAppUserResponseDto()
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
