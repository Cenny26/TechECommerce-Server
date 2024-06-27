using MediatR;

namespace TechECommerceServer.Application.Features.Commands.AppUser.FacebookLogInAppUser
{
    public class FacebookLogInAppUserCommandRequest : IRequest<FacebookLogInAppUserCommandResponse>
    {
        public string AuthToken { get; set; }
    }
}
