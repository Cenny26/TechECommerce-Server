using TechECommerceServer.Application.Bases;

namespace TechECommerceServer.Application.Features.Commands.AppUser.Exceptions
{
    public class UserLoginAuthenticationProcessException : BaseException
    {
        public UserLoginAuthenticationProcessException(string message) : base(message)
        {
        }
    }
}
