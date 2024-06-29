using AutoMapper;
using MediatR;
using TechECommerceServer.Application.Abstractions.Services.Authentications.Base;
using TechECommerceServer.Application.Bases;
using TechECommerceServer.Domain.DTOs.AppUser;

namespace TechECommerceServer.Application.Features.Commands.AppUser.LogInAppUser
{
    public class LogInAppUserCommandHandler : BaseHandler, IRequestHandler<LogInAppUserCommandRequest, LogInAppUserCommandResponse>
    {
        private readonly IAuthService _authService;
        public LogInAppUserCommandHandler(IMapper _mapper, IAuthService authService) : base(_mapper)
        {
            _authService = authService;
        }

        public async Task<LogInAppUserCommandResponse> Handle(LogInAppUserCommandRequest request, CancellationToken cancellationToken)
        {
            // note: map the incoming request to a DTO
            LogInAppUserRequestDto logInAppUserRequestDto = _mapper.Map<LogInAppUserRequestDto>(request);

            // note: call the service and get the response
            LogInAppUserResponseDto logInAppUserResponse = await _authService.LogInAppUserAsync(logInAppUserRequestDto, 60 * 60);

            // note: map the service response to the command response and return it to API
            LogInAppUserCommandResponse response = _mapper.Map<LogInAppUserCommandResponse>(logInAppUserResponse);
            return response;
        }
    }
}