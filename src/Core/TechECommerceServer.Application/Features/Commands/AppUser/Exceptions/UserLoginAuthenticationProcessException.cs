using TechECommerceServer.Application.Bases;

namespace TechECommerceServer.Application.Features.Commands.AppUser.Exceptions
{
    public class UserLoginAuthenticationProcessException : BaseException
    {
        public UserLoginAuthenticationProcessException() : base("An unexpected error was encountered during the authentication process!")
        {
        }
    }
}
