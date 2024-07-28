using AutoMapper;
using TechECommerceServer.Application.Features.Commands.AppUser.CreateAppUser;
using TechECommerceServer.Application.Features.Commands.AppUser.UpdatePassword;
using TechECommerceServer.Domain.DTOs.AppUser;
using TechECommerceServer.Domain.DTOs.AppUser.UpdatePassword;

namespace TechECommerceServer.Infrastructure.Services.AutoMapper.Profiles.AppUser
{
    public class AppUserProfile : Profile
    {
        public AppUserProfile()
        {
            CreateMap<CreateAppUserCommandRequest, CreateAppUserRequestDto>().ReverseMap();
            CreateMap<CreateAppUserCommandResponse, CreateAppUserResponseDto>().ReverseMap();

            CreateMap<UpdatePasswordCommandRequest, UpdatePasswordRequestDto>().ReverseMap();
        }
    }
}
