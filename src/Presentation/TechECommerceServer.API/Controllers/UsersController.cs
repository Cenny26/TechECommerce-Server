using MediatR;
using Microsoft.AspNetCore.Mvc;
using TechECommerceServer.Application.Features.Commands.AppUser.CreateAppUser;
using TechECommerceServer.Application.Features.Commands.AppUser.GoogleLogInAppUser;
using TechECommerceServer.Application.Features.Commands.AppUser.LoginAppUser;

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
        public async Task<IActionResult> LogInAppUser([FromBody] LoginAppUserCommandRequest loginAppUserCommandRequest)
        {
            LoginAppUserCommandResponse response = await _mediator.Send(loginAppUserCommandRequest);
            return Ok(response);
        }

        [HttpPost] // POST: api/Users/GoogleLogInAppUser
        public async Task<IActionResult> GoogleLogInAppUser([FromBody] GoogleLogInAppUserCommandRequest googleLogInAppUserRequest)
        {
            GoogleLogInAppUserCommandResponse response = await _mediator.Send(googleLogInAppUserRequest);
            return Ok(response);
        }
    }
}
