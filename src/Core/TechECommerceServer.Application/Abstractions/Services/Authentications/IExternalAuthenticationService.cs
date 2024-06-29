using TechECommerceServer.Domain.DTOs.Auth.Facebook;

namespace TechECommerceServer.Application.Abstractions.Services.Authentications
{
    public interface IExternalAuthenticationService
    {
        Task<FacebookLogInAppUserResponseDto> FacebookLogInAppUserAsync(FacebookLogInAppUserRequestDto model, int accessTokenLifeTime);
    }
}
