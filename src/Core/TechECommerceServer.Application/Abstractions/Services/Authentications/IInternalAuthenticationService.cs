using TechECommerceServer.Domain.DTOs.AppUser;

namespace TechECommerceServer.Application.Abstractions.Services.Authentications
{
    public interface IInternalAuthenticationService
    {
        Task<LogInAppUserResponseDto> LogInAppUserAsync(LogInAppUserRequestDto model, int accessTokenLifeTime);
        Task<Domain.DTOs.Auth.Token> RefreshTokenLogInAsync(string refreshToken);
    }
}
