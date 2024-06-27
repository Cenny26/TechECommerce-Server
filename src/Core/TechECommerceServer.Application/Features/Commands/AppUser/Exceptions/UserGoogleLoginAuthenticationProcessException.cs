using TechECommerceServer.Application.Bases;

namespace TechECommerceServer.Application.Features.Commands.AppUser.Exceptions
{
    public class UserGoogleLoginAuthenticationProcessException : BaseException
    {
        public UserGoogleLoginAuthenticationProcessException() : base("An unexpected error was encountered during the external (Google) authentication process!")
        {
        }
    }
}
