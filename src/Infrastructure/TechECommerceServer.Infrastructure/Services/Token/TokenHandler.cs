using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TechECommerceServer.Application.Abstractions.Token;

namespace TechECommerceServer.Infrastructure.Services.Token
{
    public class TokenHandler : ITokenHandler
    {
        private readonly IConfiguration _configuration;
        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Domain.DTOs.Auth.Token CreateAccessToken(int seconds)
        {
            Domain.DTOs.Auth.Token token = new Domain.DTOs.Auth.Token();

            // note: getting the symmetry of the 'SecurityKey'
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Token:SecurityKey"]));

            // note: generating encrypted identity
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // note: giving the token settings to be created
            token.ExpirationDate = DateTime.UtcNow.AddSeconds(seconds);
            JwtSecurityToken securityToken = new JwtSecurityToken(
                audience: _configuration["JWT:Token:Audience"],
                issuer: _configuration["JWT:Token:Issuer"],
                expires: token.ExpirationDate,
                notBefore: DateTime.UtcNow,
                signingCredentials: signingCredentials);

            // note: here is an example of the token generator class
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            token.AccessToken = tokenHandler.WriteToken(securityToken);
            return token;
        }
    }
}
