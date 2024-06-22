using TechECommerceServer.Application.Bases;

namespace TechECommerceServer.Application.Features.Commands.AppUser.Exceptions
{
    public class UserAlreadyExistsException : BaseException
    {
        public UserAlreadyExistsException(string email) : base($"A user with email: {email} is already exists!")
        {
        }
    }
}
