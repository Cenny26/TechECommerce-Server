﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using TechECommerceServer.Application.Features.Commands.AppUser.CreateAppUser;

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
    }
}
