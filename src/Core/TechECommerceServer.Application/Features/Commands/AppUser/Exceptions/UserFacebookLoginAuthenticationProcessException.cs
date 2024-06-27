using TechECommerceServer.Application.Bases;

namespace TechECommerceServer.Application.Features.Commands.AppUser.Exceptions
{
    public class UserFacebookLoginAuthenticationProcessException : BaseException
    {
        public UserFacebookLoginAuthenticationProcessException(string message) : base(message)
        {
        }
    }
}
