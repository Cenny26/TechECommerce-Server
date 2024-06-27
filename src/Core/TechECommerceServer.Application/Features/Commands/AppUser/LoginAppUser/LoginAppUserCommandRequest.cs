using MediatR;

namespace TechECommerceServer.Application.Features.Commands.AppUser.LogInAppUser
{
    public class LogInAppUserCommandRequest : IRequest<LogInAppUserCommandResponse>
    {
        public string UserNameOrEmail { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
