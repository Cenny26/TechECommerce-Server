using TechECommerceServer.Domain.DTOs.Auth.PasswordReset;
using TechECommerceServer.Domain.DTOs.Auth.VerifyResetToken;

namespace TechECommerceServer.Application.Abstractions.Services.Authentications.Base
{
    public interface IAuthService : IInternalAuthenticationService, IExternalAuthenticationService
    {
        Task PasswordResetAsync(PasswordResetRequestDto model);
        Task<VerifyResetTokenResponseDto> VerifyResetTokenAsync(VerifyResetTokenRequestDto model);
    }
}
