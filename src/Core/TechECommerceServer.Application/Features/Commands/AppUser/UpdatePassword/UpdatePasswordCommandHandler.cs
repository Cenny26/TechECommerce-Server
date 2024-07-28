using AutoMapper;
using MediatR;
using TechECommerceServer.Application.Abstractions.Services.AppUser;
using TechECommerceServer.Domain.DTOs.AppUser.UpdatePassword;

namespace TechECommerceServer.Application.Features.Commands.AppUser.UpdatePassword
{
    public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommandRequest, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IAppUserService _appUserService;
        public UpdatePasswordCommandHandler(IMapper mapper, IAppUserService appUserService)
        {
            _mapper = mapper;
            _appUserService = appUserService;
        }

        public async Task<Unit> Handle(UpdatePasswordCommandRequest request, CancellationToken cancellationToken)
        {
            // note: map the incoming request to a DTO
            UpdatePasswordRequestDto updatePasswordRequestDto = _mapper.Map<UpdatePasswordRequestDto>(request);

            // note: call the service
            await _appUserService.UpdateAppUserPasswordAsync(model: updatePasswordRequestDto);

            return Unit.Value;
        }
    }
}
