using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TechECommerceServer.Application.Features.Commands.Product.CreateProduct;

namespace TechECommerceServer.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost] // POST: api/Products/CreateProduct
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommandRequest createProductCommandRequest)
        {
            await _mediator.Send(createProductCommandRequest);
            return StatusCode((int)HttpStatusCode.Created);
        }
    }
}
