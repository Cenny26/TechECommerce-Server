using AutoMapper;
using MediatR;
using TechECommerceServer.Application.Abstractions.Services.AppUser;
using TechECommerceServer.Application.Bases;
using TechECommerceServer.Domain.DTOs.AppUser;

namespace TechECommerceServer.Application.Features.Commands.AppUser.CreateAppUser
{
    public class CreateAppUserCommandHandler : BaseHandler, IRequestHandler<CreateAppUserCommandRequest, CreateAppUserCommandResponse>
    {
        private readonly IAppUserService _appUserService;
        public CreateAppUserCommandHandler(IMapper _mapper, IAppUserService appUserService) : base(_mapper)
        {
            _appUserService = appUserService;
        }

        public async Task<CreateAppUserCommandResponse> Handle(CreateAppUserCommandRequest request, CancellationToken cancellationToken)
        {
            // note: map the incoming request to a DTO
            CreateAppUserRequestDto createAppUserRequestDto = _mapper.Map<CreateAppUserRequestDto>(request);

            // note: call the service to create the app user and get the response
            CreateAppUserResponseDto createAppUserResponse = await _appUserService.CreateAppUserAsync(createAppUserRequestDto);

            // note: map the service response to the command response and return it to API
            CreateAppUserCommandResponse response = _mapper.Map<CreateAppUserCommandResponse>(createAppUserResponse);
            return response;
        }
    }
}
