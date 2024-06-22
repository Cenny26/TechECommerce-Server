using Microsoft.AspNetCore.Identity;

namespace TechECommerceServer.Domain.Entities.Identity
{
    public class AppUser : IdentityUser<string>
    {
        public string FullName { get; set; }
    }
}
