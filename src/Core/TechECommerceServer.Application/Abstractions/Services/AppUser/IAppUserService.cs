using TechECommerceServer.Domain.DTOs.AppUser;
using TechECommerceServer.Domain.DTOs.AppUser.UpdatePassword;

namespace TechECommerceServer.Application.Abstractions.Services.AppUser
{
    public interface IAppUserService
    {
        Task<CreateAppUserResponseDto> CreateAppUserAsync(CreateAppUserRequestDto model);
        Task UpdateRefreshToken(string refreshToken, Domain.Entities.Identity.AppUser appUser, DateTime accessTokenDate, int addOnAccessTokenDate);
        Task UpdateAppUserPasswordAsync(UpdatePasswordRequestDto model);
    }
}
