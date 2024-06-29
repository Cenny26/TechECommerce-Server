using AutoMapper;
using TechECommerceServer.Application.Features.Commands.AppUser.FacebookLogInAppUser;
using TechECommerceServer.Domain.DTOs.Auth.Facebook;

namespace TechECommerceServer.Infrastructure.Services.AutoMapper.Profiles.Auth
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<FacebookLogInAppUserCommandRequest, FacebookLogInAppUserRequestDto>().ReverseMap();
            CreateMap<FacebookLogInAppUserCommandResponse, FacebookLogInAppUserResponseDto>().ReverseMap();
        }
    }
}
