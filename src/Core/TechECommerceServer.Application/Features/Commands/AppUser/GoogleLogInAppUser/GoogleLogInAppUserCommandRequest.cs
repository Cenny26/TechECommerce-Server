using MediatR;

namespace TechECommerceServer.Application.Features.Commands.AppUser.GoogleLogInAppUser
{
    public class GoogleLogInAppUserCommandRequest : IRequest<GoogleLogInAppUserCommandResponse>
    {
        public string IdToken { get; set; }
    }
}
