using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TechECommerceServer.Application.Bases;
using TechECommerceServer.Application.Features.Commands.AppUser.Rules;

namespace TechECommerceServer.Application.Features.Commands.AppUser.CreateAppUser
{
    public class CreateAppUserCommandHandler : BaseHandler, IRequestHandler<CreateAppUserCommandRequest, CreateAppUserCommandResponse>
    {
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        private readonly BaseUserRules _userRules;
        public CreateAppUserCommandHandler(IMapper Mapper, UserManager<Domain.Entities.Identity.AppUser> userManager, BaseUserRules userRules) : base(Mapper)
        {
            _userManager = userManager;
            _userRules = userRules;
        }

        public async Task<CreateAppUserCommandResponse> Handle(CreateAppUserCommandRequest request, CancellationToken cancellationToken)
        {
            await _userRules.UserEmailShouldBeUnique(appUser: await _userManager.FindByEmailAsync(request.Email), email: request.Email);

            try
            {
                // todo: need to use AutoMapper for bind automatic!
                Guid newUserId = Guid.NewGuid();
                IdentityResult identityResult = await _userManager.CreateAsync(new Domain.Entities.Identity.AppUser()
                {
                    Id = newUserId.ToString(),
                    FullName = request.FullName,
                    UserName = request.UserName,
                    Email = request.Email,
                    EmailConfirmed = request.Email is not null
                }, request.Password);

                CreateAppUserCommandResponse response = new CreateAppUserCommandResponse()
                {
                    IsSucceed = identityResult.Succeeded
                };

                if (identityResult.Succeeded)
                    response.Message = $"The new user with id: {newUserId} was successfully created!";
                else
                    foreach (IdentityError error in identityResult.Errors)
                        response.Message = $"{error.Code} - {error.Description}\n";

                return response;
            }
            catch (Exception exc)
            {
                throw new OperationCanceledException("An unexpected error was encountered while creating the user!", exc);
            }
        }
    }
}
