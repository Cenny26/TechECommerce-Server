using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using TechECommerceServer.Application.Abstractions.Services.Authentications.Base;
using TechECommerceServer.Application.Abstractions.Token;
using TechECommerceServer.Application.Features.Commands.AppUser.Exceptions;
using TechECommerceServer.Application.Features.Commands.AppUser.Rules;
using TechECommerceServer.Domain.DTOs.AppUser;
using TechECommerceServer.Domain.DTOs.Auth;
using TechECommerceServer.Domain.DTOs.Auth.Facebook;
using TechECommerceServer.Domain.DTOs.Auth.Google;
using TechECommerceServer.Persistence.Concretes.Services.Authentications.Utils;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace TechECommerceServer.Persistence.Concretes.Services.Authentications.Base
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly BaseUserRules _userRules;
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        private readonly ITokenHandler _tokenHandler;
        private readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;
        private readonly HttpClient _httpClient;
        public AuthService(IConfiguration configuration, BaseUserRules userRules, IHttpClientFactory httpClientFactory, UserManager<Domain.Entities.Identity.AppUser> userManager, ITokenHandler tokenHandler, SignInManager<Domain.Entities.Identity.AppUser> signInManager)
        {
            _configuration = configuration;
            _userRules = userRules;
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _signInManager = signInManager;
            _httpClient = httpClientFactory.CreateClient();
        }

        private async Task<Token> CreateExternalAppUserAsync(Domain.Entities.Identity.AppUser appUser, UserLoginInfo loginInfo, string email, string name, int accessTokenLifeTime)
        {
            bool userResult = appUser is not null;
            if (appUser is null)
            {
                appUser = await _userManager.FindByEmailAsync(email);
                if (appUser is null)
                {
                    appUser = new Domain.Entities.Identity.AppUser()
                    {
                        Id = Guid.NewGuid().ToString(),
                        FullName = name,
                        UserName = name,
                        Email = email
                    };

                    IdentityResult identityResult = await _userManager.CreateAsync(appUser);
                    userResult = identityResult.Succeeded;
                }
            }

            if (userResult)
                await _userManager.AddLoginAsync(appUser, loginInfo);

            Token token = _tokenHandler.CreateAccessToken(seconds: accessTokenLifeTime); // note: default 60 minute for expire!
            return token;
        }

        public async Task<FacebookLogInAppUserResponseDto> FacebookLogInAppUserAsync(FacebookLogInAppUserRequestDto model, int accessTokenLifeTime)
        {
            string provider = _configuration["ExternalLoginSettings:OAuth:Facebook:Provider"] ?? "FACEBOOK"; // note: 'FACEBOOK'

            string clientId = _configuration["ExternalLoginSettings:OAuth:Facebook:ClientId"]!;
            string clientSecret = _configuration["ExternalLoginSettings:OAuth:Facebook:ClientSecret"]!;
            string grantType = _configuration["ExternalLoginSettings:OAuth:Facebook:GrantType"]!;

            // note: replacing the validation class with a simple rule instead
            await _userRules.GivenTokenMustBeLoadWhenProcessToExternalLogIn(model.AuthToken, provider);

            try
            {
                #region Meta for Developers
                // note: let search on: 'https://developers.facebook.com/docs/facebook-login/guides' for more information
                #endregion

                // note: curl -X GET "https://graph.facebook.com/oauth/access_token?client_id={your-app-id}&client_secret={your-app-secret}&grant_type=client_credentials"
                string accessTokenUrl = FacebookApiService.BuildAccessTokenUrl(clientId, clientSecret, grantType);
                string accessTokenResponse = await _httpClient.GetStringAsync(accessTokenUrl);

                FacebookAccessTokenResponseDto? facebookAccessTokenResponse = JsonSerializer.Deserialize<FacebookAccessTokenResponseDto>(accessTokenResponse);

                // note: curl -i -X GET "https://graph.facebook.com/debug_token?input_token={input-token}&access_token={valid-access-token}
                string userAccessTokenValidationUrl = FacebookApiService.BuildUserAccessTokenValidationUrl(model.AuthToken, facebookAccessTokenResponse?.AccessToken);
                string userAccessTokenValidationResponse = await _httpClient.GetStringAsync(userAccessTokenValidationUrl);

                FacebookUserAccessTokenValidationResponseDto? facebookUserAccessTokenValidationResponse = JsonSerializer.Deserialize<FacebookUserAccessTokenValidationResponseDto>(userAccessTokenValidationResponse);

                if (facebookUserAccessTokenValidationResponse?.Data.IsValid is not null)
                {
                    // note: curl -i -X GET "https://graph.facebook.com/USER-ID?fields=id,name,email,picture&access_token=ACCESS-TOKEN"
                    string userInfoUrl = FacebookApiService.BuildUserInfoUrl(model.AuthToken);
                    string userInfoResponse = await _httpClient.GetStringAsync(userInfoUrl);

                    FacebookUserInfoResponseDto? facebookUserInfoResponse = JsonSerializer.Deserialize<FacebookUserInfoResponseDto>(userInfoResponse);

                    UserLoginInfo userLoginInfo = new UserLoginInfo(provider, facebookUserAccessTokenValidationResponse.Data.UserId, provider);
                    Domain.Entities.Identity.AppUser? appUser = await _userManager.FindByLoginAsync(userLoginInfo.LoginProvider, userLoginInfo.ProviderKey);

                    Token token = await CreateExternalAppUserAsync(appUser, userLoginInfo, facebookUserInfoResponse.Email, facebookUserInfoResponse.Name, accessTokenLifeTime);
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

        public async Task<GoogleLogInAppUserResponseDto> GoogleLogInAppUserAsync(GoogleLogInAppUserRequestDto model, int accessTokenLifeTime)
        {
            string provider = _configuration["ExternalLoginSettings:OAuth:Google:Provider"] ?? "GOOGLE"; // note: 'GOOGLE'

            // note: replacing the validation class with a simple rule instead
            await _userRules.GivenTokenMustBeLoadWhenProcessToExternalLogIn(model.IdToken, provider);

            try
            {
                ValidationSettings validationSettings = new ValidationSettings()
                {
                    Audience = new List<string> { _configuration["ExternalLoginSettings:OAuth:Google:ClientId"] }
                };

                Payload payload = await ValidateAsync(model.IdToken, validationSettings);

                UserLoginInfo userLoginInfo = new UserLoginInfo(provider, payload.Subject, provider);
                Domain.Entities.Identity.AppUser? appUser = await _userManager.FindByLoginAsync(userLoginInfo.LoginProvider, userLoginInfo.ProviderKey);

                Token token = await CreateExternalAppUserAsync(appUser, userLoginInfo, payload.Email, payload.Name, accessTokenLifeTime);
                return new GoogleLogInAppUserResponseDto()
                {
                    Token = token
                };
            }
            catch (Exception exc)
            {
                throw new UserGoogleLoginAuthenticationProcessException();
            }
        }

        public async Task<LogInAppUserResponseDto> LogInAppUserAsync(LogInAppUserRequestDto model, int accessTokenLifeTime)
        {
            Domain.Entities.Identity.AppUser? user = await _userManager.FindByNameAsync(model.UserNameOrEmail) ?? await _userManager.FindByEmailAsync(model.UserNameOrEmail);
            // note: replacing the validation class with a simple rule instead
            await _userRules.GivenAppUserMustBeLoadWhenProcessToLogIn(user);

            try
            {
                SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);
                if (signInResult.Succeeded) // note: authentication was successful
                {
                    Token token = _tokenHandler.CreateAccessToken(seconds: accessTokenLifeTime); // note: default 60 minute for expire!
                    return new LogInAppUserResponseDto()
                    {
                        Token = token
                    };
                }

                throw new UserLoginAuthenticationProcessException("The user was not verified during sign in process!");
            }
            catch (Exception exc)
            {
                throw new UserLoginAuthenticationProcessException("An unexpected error was encountered during the authentication process!");
            }
        }
    }
}
