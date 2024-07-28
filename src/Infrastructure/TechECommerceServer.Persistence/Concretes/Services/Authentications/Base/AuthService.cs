using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TechECommerceServer.Application.Abstractions.Mail;
using TechECommerceServer.Application.Abstractions.Services.AppUser;
using TechECommerceServer.Application.Abstractions.Services.Authentications.Base;
using TechECommerceServer.Application.Abstractions.Token;
using TechECommerceServer.Application.Abstractions.Token.Utils;
using TechECommerceServer.Application.Features.Commands.AppUser.Exceptions;
using TechECommerceServer.Application.Features.Commands.AppUser.Rules;
using TechECommerceServer.Application.Helpers.UrlCoding;
using TechECommerceServer.Domain.DTOs.AppUser;
using TechECommerceServer.Domain.DTOs.Auth;
using TechECommerceServer.Domain.DTOs.Auth.Facebook;
using TechECommerceServer.Domain.DTOs.Auth.Google;
using TechECommerceServer.Domain.DTOs.Auth.PasswordReset;
using TechECommerceServer.Domain.DTOs.Auth.VerifyResetToken;
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
        private readonly IAppUserService _appUserService;
        private readonly ILogger<AuthService> _logger;
        private readonly IMailService _mailService;
        private readonly HttpClient _httpClient;
        public AuthService(IConfiguration configuration, BaseUserRules userRules, IHttpClientFactory httpClientFactory, UserManager<Domain.Entities.Identity.AppUser> userManager, ITokenHandler tokenHandler, SignInManager<Domain.Entities.Identity.AppUser> signInManager, IAppUserService appUserService, ILogger<AuthService> logger, IMailService mailService)
        {
            _configuration = configuration;
            _userRules = userRules;
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _signInManager = signInManager;
            _appUserService = appUserService;
            _logger = logger;
            _mailService = mailService;
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

            Token token = _tokenHandler.CreateAccessToken(seconds: accessTokenLifeTime, appUser); // note: default 60 minute for expire!
            await _appUserService.UpdateRefreshToken(token.RefreshToken, appUser, token.ExpirationDate, addOnAccessTokenDate: DefaultTokenVariables.StandardRefreshTokenValueBySeconds);
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
                    Token token = _tokenHandler.CreateAccessToken(seconds: accessTokenLifeTime, user); // note: default 60 minute for expire!
                    await _appUserService.UpdateRefreshToken(token.RefreshToken, user, token.ExpirationDate, addOnAccessTokenDate: DefaultTokenVariables.StandardRefreshTokenValueBySeconds);
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

        public async Task<Token> RefreshTokenLogInAsync(string refreshToken)
        {
            Domain.Entities.Identity.AppUser? appUser = await _userManager.Users.FirstOrDefaultAsync(user => user.RefreshToken == refreshToken);
            if (appUser is not null && appUser?.RefreshTokenEndDate > DateTime.UtcNow)
            {
                Token token = _tokenHandler.CreateAccessToken(seconds: DefaultTokenVariables.StandardAccessTokenValueBySeconds, appUser);
                await _appUserService.UpdateRefreshToken(token.RefreshToken, appUser, token.ExpirationDate, addOnAccessTokenDate: DefaultTokenVariables.StandardRefreshTokenValueBySeconds);
                return token;
            }
            else
                throw new NotUserFoundedException();
        }

        public async Task PasswordResetAsync(PasswordResetRequestDto model)
        {
            _logger.LogDebug("Entering {MethodName} with email: {Email}", nameof(PasswordResetAsync), model.Email);

            Domain.Entities.Identity.AppUser? appUser = await _userManager.FindByEmailAsync(model.Email);

            try
            {
                if (appUser is not null)
                {
                    string resetToken = await _userManager.GeneratePasswordResetTokenAsync(appUser);
                    resetToken = resetToken.UrlEncode();

                    await _mailService.SendPasswordResetDemandMailAsync(appUser.Email!, appUser.Id, resetToken);
                }
                else
                {
                    throw new ArgumentNullException($"No user found with email: {model.Email}", nameof(model.Email));
                }
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, "An error occurred while resetting password for email: {Email}", model.Email);
                throw new Exception($"An error occurred while resetting password for email: {model.Email}", exc);
            }
            finally
            {
                _logger.LogDebug("Exiting {MethodName}", nameof(PasswordResetAsync));
            }
        }

        public async Task<VerifyResetTokenResponseDto> VerifyResetTokenAsync(VerifyResetTokenRequestDto model)
        {
            _logger.LogDebug("Entering {MethodName} with user ID: {UserId}", nameof(VerifyResetTokenAsync), model.UserId);

            Domain.Entities.Identity.AppUser? appUser = await _userManager.FindByIdAsync(model.UserId);

            try
            {
                if (appUser is not null)
                {
                    model.ResetToken = model.ResetToken.UrlDecode();

                    bool state = await _userManager.VerifyUserTokenAsync(appUser, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", model.ResetToken);
                    return new VerifyResetTokenResponseDto()
                    {
                        State = state
                    };
                }

                return new VerifyResetTokenResponseDto() { State = false };
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, "An error occurred while verifying reset token value: {resetToken}", model.ResetToken);
                throw new Exception($"An error occurred while verifying reset token value: {model.ResetToken}", exc);
            }
            finally
            {
                _logger.LogDebug("Exiting {MethodName}", nameof(PasswordResetAsync));
            }
        }
    }
}
