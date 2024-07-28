using AutoMapper;
using TechECommerceServer.Application.Features.Commands.AppUser.FacebookLogInAppUser;
using TechECommerceServer.Application.Features.Commands.AppUser.GoogleLogInAppUser;
using TechECommerceServer.Application.Features.Commands.AppUser.LogInAppUser;
using TechECommerceServer.Application.Features.Commands.AppUser.PasswordReset;
using TechECommerceServer.Domain.DTOs.AppUser;
using TechECommerceServer.Domain.DTOs.Auth.Facebook;
using TechECommerceServer.Domain.DTOs.Auth.Google;
using TechECommerceServer.Domain.DTOs.Auth.PasswordReset;

namespace TechECommerceServer.Infrastructure.Services.AutoMapper.Profiles.Auth
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<FacebookLogInAppUserCommandRequest, FacebookLogInAppUserRequestDto>().ReverseMap();
            CreateMap<FacebookLogInAppUserCommandResponse, FacebookLogInAppUserResponseDto>().ReverseMap();

            CreateMap<GoogleLogInAppUserCommandRequest, GoogleLogInAppUserRequestDto>().ReverseMap();
            CreateMap<GoogleLogInAppUserCommandResponse, GoogleLogInAppUserResponseDto>().ReverseMap();

            CreateMap<LogInAppUserCommandRequest, LogInAppUserRequestDto>().ReverseMap();
            CreateMap<LogInAppUserCommandResponse, LogInAppUserResponseDto>().ReverseMap();

            CreateMap<PasswordResetCommandRequest, PasswordResetRequestDto>().ReverseMap();
        }
    }
}
