using AutoMapper;
using MediatR;
using TechECommerceServer.Application.Abstractions.Services.Authentications.Base;
using TechECommerceServer.Domain.DTOs.Auth.VerifyResetToken;

namespace TechECommerceServer.Application.Features.Commands.AppUser.VerifyResetToken
{
    public class VerifyResetTokenCommandHandler : IRequestHandler<VerifyResetTokenCommandRequest, VerifyResetTokenCommandResponse>
    {
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public VerifyResetTokenCommandHandler(IMapper mapper, IAuthService authService)
        {
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<VerifyResetTokenCommandResponse> Handle(VerifyResetTokenCommandRequest request, CancellationToken cancellationToken)
        {
            // note: map the incoming request to a DTO
            VerifyResetTokenRequestDto verifyResetTokenRequestDto = _mapper.Map<VerifyResetTokenRequestDto>(request);

            // note: call the service and get the response
            VerifyResetTokenResponseDto verifyResetTokenResponseDto = await _authService.VerifyResetTokenAsync(model: verifyResetTokenRequestDto);

            // note: map the service response to the command response and return it to API
            VerifyResetTokenCommandResponse response = _mapper.Map<VerifyResetTokenCommandResponse>(verifyResetTokenResponseDto);
            return response;
        }
    }
}
