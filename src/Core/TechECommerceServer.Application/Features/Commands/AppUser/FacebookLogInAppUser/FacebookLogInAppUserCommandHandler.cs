using AutoMapper;
using MediatR;
using TechECommerceServer.Application.Abstractions.Services.Authentications.Base;
using TechECommerceServer.Application.Bases;
using TechECommerceServer.Domain.DTOs.Auth.Facebook;

namespace TechECommerceServer.Application.Features.Commands.AppUser.FacebookLogInAppUser
{
    public class FacebookLogInAppUserCommandHandler : BaseHandler, IRequestHandler<FacebookLogInAppUserCommandRequest, FacebookLogInAppUserCommandResponse>
    {
        private readonly IAuthService _authService;
        public FacebookLogInAppUserCommandHandler(IMapper _mapper, IAuthService authService) : base(_mapper)
        {
            _authService = authService;
        }

        public async Task<FacebookLogInAppUserCommandResponse> Handle(FacebookLogInAppUserCommandRequest request, CancellationToken cancellationToken)
        {
            // note: map the incoming request to a DTO
            FacebookLogInAppUserRequestDto facebookLogInAppUserRequestDto = _mapper.Map<FacebookLogInAppUserRequestDto>(request);

            // note: call the service and get the response
            FacebookLogInAppUserResponseDto facebookLogInAppUserResponse = await _authService.FacebookLogInAppUserAsync(model: facebookLogInAppUserRequestDto, accessTokenLifeTime: 60 * 60);

            // note: map the service response to the command response and return it to API
            FacebookLogInAppUserCommandResponse response = _mapper.Map<FacebookLogInAppUserCommandResponse>(facebookLogInAppUserResponse);
            return response;
        }
    }
}
