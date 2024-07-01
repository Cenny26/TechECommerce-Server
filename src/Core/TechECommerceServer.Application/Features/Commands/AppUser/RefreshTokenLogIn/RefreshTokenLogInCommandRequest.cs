using MediatR;

namespace TechECommerceServer.Application.Features.Commands.AppUser.RefreshTokenLogIn
{
    public class RefreshTokenLogInCommandRequest : IRequest<RefreshTokenLogInCommandResponse>
    {
        public string RefreshToken { get; set; }
    }
}
