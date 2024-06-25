using MediatR;

namespace TechECommerceServer.Application.Features.Commands.AppUser.LoginAppUser
{
    public class LoginAppUserCommandRequest : IRequest<LoginAppUserCommandResponse>
    {
        public string UserNameOrEmail { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
