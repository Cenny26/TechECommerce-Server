using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TechECommerceServer.Application.Abstractions.Token;
using TechECommerceServer.Application.Abstractions.Token.Utils;
using TechECommerceServer.Application.Bases;
using TechECommerceServer.Application.Features.Commands.AppUser.Exceptions;
using TechECommerceServer.Application.Features.Commands.AppUser.Rules;
using TechECommerceServer.Domain.DTOs.Auth;

namespace TechECommerceServer.Application.Features.Commands.AppUser.LogInAppUser
{
    public class LogInAppUserCommandHandler : BaseHandler, IRequestHandler<LogInAppUserCommandRequest, LogInAppUserCommandResponse>
    {
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        private readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;
        private readonly BaseUserRules _userRules;
        private readonly ITokenHandler _tokenHandler;
        public LogInAppUserCommandHandler(IMapper _mapper, UserManager<Domain.Entities.Identity.AppUser> userManager, SignInManager<Domain.Entities.Identity.AppUser> signInManager, BaseUserRules userRules, ITokenHandler tokenHandler) : base(_mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRules = userRules;
            _tokenHandler = tokenHandler;
        }

        public async Task<LogInAppUserCommandResponse> Handle(LogInAppUserCommandRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Identity.AppUser? user = await _userManager.FindByNameAsync(request.UserNameOrEmail) ?? await _userManager.FindByEmailAsync(request.UserNameOrEmail);
            await _userRules.GivenAppUserMustBeLoadWhenProcessToLogIn(user);

            SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
            if (signInResult.Succeeded) // note: authentication was successful
            {
                Token token = _tokenHandler.CreateAccessToken(DefaultTokenVariables.StandardTokenValue); // note: default 60 minute for expire!
                return new LogInAppUserCommandResponse()
                {
                    Token = token
                };
            }

            throw new UserLoginAuthenticationProcessException();
        }
    }
}
