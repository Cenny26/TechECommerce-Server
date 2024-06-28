using AutoMapper;
using TechECommerceServer.Application.Features.Commands.AppUser.CreateAppUser;
using TechECommerceServer.Domain.DTOs.AppUser;

namespace TechECommerceServer.Infrastructure.Services.AutoMapper.Profiles.AppUser
{
    public class AppUserProfile : Profile
    {
        public AppUserProfile()
        {
            CreateMap<CreateAppUserCommandRequest, CreateAppUserRequestDto>().ReverseMap();
            CreateMap<CreateAppUserCommandResponse, CreateAppUserResponseDto>().ReverseMap();
        }
    }
}
