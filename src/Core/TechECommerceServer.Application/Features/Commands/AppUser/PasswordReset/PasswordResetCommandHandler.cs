using AutoMapper;
using MediatR;
using TechECommerceServer.Application.Abstractions.Services.Authentications.Base;
using TechECommerceServer.Domain.DTOs.Auth.PasswordReset;

namespace TechECommerceServer.Application.Features.Commands.AppUser.PasswordReset
{
    public class PasswordResetCommandHandler : IRequestHandler<PasswordResetCommandRequest, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public PasswordResetCommandHandler(IMapper mapper, IAuthService authService)
        {
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<Unit> Handle(PasswordResetCommandRequest request, CancellationToken cancellationToken)
        {
            // note: map the incoming request to a DTO
            PasswordResetRequestDto passwordResetRequestDto = _mapper.Map<PasswordResetRequestDto>(request);

            // note: call the service
            await _authService.PasswordResetAsync(model: passwordResetRequestDto);

            return Unit.Value;
        }
    }
}
