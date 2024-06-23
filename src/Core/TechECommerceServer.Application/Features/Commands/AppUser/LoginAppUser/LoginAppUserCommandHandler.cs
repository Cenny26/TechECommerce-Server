using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TechECommerceServer.Application.Bases;
using TechECommerceServer.Application.Features.Commands.AppUser.Rules;

namespace TechECommerceServer.Application.Features.Commands.AppUser.LoginAppUser
{
    public class LoginAppUserCommandHandler : BaseHandler, IRequestHandler<LoginAppUserCommandRequest, Unit>
    {
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        private readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;
        private readonly BaseUserRules _userRules;
        public LoginAppUserCommandHandler(IMapper _mapper, UserManager<Domain.Entities.Identity.AppUser> userManager, SignInManager<Domain.Entities.Identity.AppUser> signInManager, BaseUserRules userRules) : base(_mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRules = userRules;
        }

        public async Task<Unit> Handle(LoginAppUserCommandRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Identity.AppUser? user = await _userManager.FindByNameAsync(request.UserNameOrEmail) ?? await _userManager.FindByEmailAsync(request.UserNameOrEmail);
            await _userRules.GivenAppUserMustBeLoadWhenProcessToLogIn(user);

            SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
            if (signInResult.Succeeded) // note: authentication was successful
            {
                // todo: necessary to specify the appropriate authorities!
            }

            return Unit.Value;
        }
    }
}
