using TechECommerceServer.Domain.DTOs.Auth.PasswordReset;

namespace TechECommerceServer.Application.Abstractions.Services.Authentications.Base
{
    public interface IAuthService : IInternalAuthenticationService, IExternalAuthenticationService
    {
        Task PasswordResetAsync(PasswordResetRequestDto model);
    }
}
