using TechECommerceServer.Domain.DTOs.Auth.Facebook;
using TechECommerceServer.Domain.DTOs.Auth.Google;

namespace TechECommerceServer.Application.Abstractions.Services.Authentications
{
    public interface IExternalAuthenticationService
    {
        Task<FacebookLogInAppUserResponseDto> FacebookLogInAppUserAsync(FacebookLogInAppUserRequestDto model, int accessTokenLifeTime);
        Task<GoogleLogInAppUserResponseDto> GoogleLogInAppUserAsync(GoogleLogInAppUserRequestDto model, int accessTokenLifeTime);
    }
}
