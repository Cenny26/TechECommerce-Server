namespace TechECommerceServer.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        Domain.DTOs.Auth.Token CreateAccessToken(int seconds);
        string CreateRefreshToken();
    }
}
