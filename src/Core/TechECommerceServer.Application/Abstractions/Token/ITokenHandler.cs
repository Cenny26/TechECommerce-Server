using TechECommerceServer.Domain.Entities.Identity;

namespace TechECommerceServer.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        Domain.DTOs.Auth.Token CreateAccessToken(int seconds, AppUser appUser);
        string CreateRefreshToken();
    }
}
