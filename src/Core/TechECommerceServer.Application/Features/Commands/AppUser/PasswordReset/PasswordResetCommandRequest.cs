using MediatR;

namespace TechECommerceServer.Application.Features.Commands.AppUser.PasswordReset
{
    public class PasswordResetCommandRequest : IRequest<Unit>
    {
        public string Email { get; set; }
    }
}
