using TechECommerceServer.Domain.DTOs.AppUser;

namespace TechECommerceServer.Application.Abstractions.Services.AppUser
{
    public interface IAppUserService
    {
        Task<CreateAppUserResponseDto> CreateAppUserAsync(CreateAppUserRequestDto model);
    }
}
