using MediatR;
using Microsoft.AspNetCore.Mvc;
using TechECommerceServer.Application.Features.Commands.AppUser.CreateAppUser;
using TechECommerceServer.Application.Features.Commands.AppUser.FacebookLogInAppUser;
using TechECommerceServer.Application.Features.Commands.AppUser.GoogleLogInAppUser;
using TechECommerceServer.Application.Features.Commands.AppUser.LogInAppUser;

namespace TechECommerceServer.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost] // POST: api/Users/CreateAppUser
        public async Task<IActionResult> CreateAppUser([FromBody] CreateAppUserCommandRequest createAppUserCommandRequest)
        {
            CreateAppUserCommandResponse response = await _mediator.Send(createAppUserCommandRequest);
            return Ok(response);
        }

        [HttpPost] // POST: api/Users/LogInAppUser
        public async Task<IActionResult> LogInAppUser([FromBody] LogInAppUserCommandRequest logInAppUserCommandRequest)
        {
            LogInAppUserCommandResponse response = await _mediator.Send(logInAppUserCommandRequest);
            return Ok(response);
        }

        [HttpPost] // POST: api/Users/GoogleLogInAppUser
        public async Task<IActionResult> GoogleLogInAppUser([FromBody] GoogleLogInAppUserCommandRequest googleLogInAppUserCommandRequest)
        {
            GoogleLogInAppUserCommandResponse response = await _mediator.Send(googleLogInAppUserCommandRequest);
            return Ok(response);
        }

        [HttpPost] // POST: api/Users/FacebookLogInAppUser
        public async Task<IActionResult> FacebookLogInAppUser([FromBody] FacebookLogInAppUserCommandRequest facebookLogInAppUserCommandRequest)
        {
            FacebookLogInAppUserCommandResponse response = await _mediator.Send(facebookLogInAppUserCommandRequest);
            return Ok(response);
        }
    }
}
