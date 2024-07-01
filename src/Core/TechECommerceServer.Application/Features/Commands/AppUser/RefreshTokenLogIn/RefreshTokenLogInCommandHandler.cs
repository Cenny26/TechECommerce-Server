using MediatR;
using TechECommerceServer.Application.Abstractions.Services.Authentications.Base;

namespace TechECommerceServer.Application.Features.Commands.AppUser.RefreshTokenLogIn
{
    public class RefreshTokenLogInCommandHandler : IRequestHandler<RefreshTokenLogInCommandRequest, RefreshTokenLogInCommandResponse>
    {
        private readonly IAuthService _authService;
        public RefreshTokenLogInCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<RefreshTokenLogInCommandResponse> Handle(RefreshTokenLogInCommandRequest request, CancellationToken cancellationToken)
        {
            Domain.DTOs.Auth.Token token = await _authService.RefreshTokenLogInAsync(request.RefreshToken);
            return new RefreshTokenLogInCommandResponse()
            {
                Token = token
            };
        }
    }
}
