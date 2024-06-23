using TechECommerceServer.Application.Bases;

namespace TechECommerceServer.Application.Features.Commands.AppUser.Exceptions
{
    public class NotUserFoundedException : BaseException
    {
        public NotUserFoundedException() : base("The entered username or password is incorrect. Check it out again!")
        {
        }
    }
}
