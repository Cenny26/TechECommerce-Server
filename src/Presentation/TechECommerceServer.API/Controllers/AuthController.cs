using MediatR;
using Microsoft.AspNetCore.Mvc;
using TechECommerceServer.Application.Features.Commands.AppUser.FacebookLogInAppUser;
using TechECommerceServer.Application.Features.Commands.AppUser.GoogleLogInAppUser;
using TechECommerceServer.Application.Features.Commands.AppUser.LogInAppUser;

namespace TechECommerceServer.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost] // POST: api/Auth/LogInAppUser
        public async Task<IActionResult> LogInAppUser([FromBody] LogInAppUserCommandRequest logInAppUserCommandRequest)
        {
            LogInAppUserCommandResponse response = await _mediator.Send(logInAppUserCommandRequest);
            return Ok(response);
        }

        [HttpPost] // POST: api/Auth/GoogleLogInAppUser
        public async Task<IActionResult> GoogleLogInAppUser([FromBody] GoogleLogInAppUserCommandRequest googleLogInAppUserCommandRequest)
        {
            GoogleLogInAppUserCommandResponse response = await _mediator.Send(googleLogInAppUserCommandRequest);
            return Ok(response);
        }

        [HttpPost] // POST: api/Auth/FacebookLogInAppUser
        public async Task<IActionResult> FacebookLogInAppUser([FromBody] FacebookLogInAppUserCommandRequest facebookLogInAppUserCommandRequest)
        {
            FacebookLogInAppUserCommandResponse response = await _mediator.Send(facebookLogInAppUserCommandRequest);
            return Ok(response);
        }
    }
}
