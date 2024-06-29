using AutoMapper;
using MediatR;
using TechECommerceServer.Application.Abstractions.Services.Authentications.Base;
using TechECommerceServer.Application.Bases;
using TechECommerceServer.Domain.DTOs.Auth.Google;

namespace TechECommerceServer.Application.Features.Commands.AppUser.GoogleLogInAppUser
{
    public class GoogleLogInAppUserCommandHandler : BaseHandler, IRequestHandler<GoogleLogInAppUserCommandRequest, GoogleLogInAppUserCommandResponse>
    {
        private readonly IAuthService _authService;
        public GoogleLogInAppUserCommandHandler(IMapper _mapper, IAuthService authService) : base(_mapper)
        {
            _authService = authService;
        }

        public async Task<GoogleLogInAppUserCommandResponse> Handle(GoogleLogInAppUserCommandRequest request, CancellationToken cancellationToken)
        {
            // note: map the incoming request to a DTO
            GoogleLogInAppUserRequestDto googleLogInAppUserRequestDto = _mapper.Map<GoogleLogInAppUserRequestDto>(request);

            // note: call the service and get the response
            GoogleLogInAppUserResponseDto googleLogInAppUserResponse = await _authService.GoogleLogInAppUserAsync(model: googleLogInAppUserRequestDto, accessTokenLifeTime: 60 * 60);

            // note: map the service response to the command response and return it to API
            GoogleLogInAppUserCommandResponse response = _mapper.Map<GoogleLogInAppUserCommandResponse>(googleLogInAppUserResponse);
            return response;
        }
    }
}
