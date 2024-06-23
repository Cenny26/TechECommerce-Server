using MediatR;

namespace TechECommerceServer.Application.Features.Commands.AppUser.LoginAppUser
{
    public class LoginAppUserCommandRequest : IRequest<Unit>
    {
        public string UserNameOrEmail { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
