using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TechECommerceServer.Application.Features.Commands.Product.CreateProduct;
using TechECommerceServer.Application.Features.Commands.Product.UpdateProduct;
using TechECommerceServer.Application.Features.Queries.Product.GetAllProducts;
using TechECommerceServer.Application.Features.Queries.Product.GetLimitedProductsByPaging;
using TechECommerceServer.Application.Features.Queries.Product.GetProductById;

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

        [HttpPut] // PUT: api/Products/UpdateProduct
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommandRequest updateProductCommandRequest)
        {
            await _mediator.Send(updateProductCommandRequest);
            return StatusCode((int)HttpStatusCode.Accepted);
        }

        [HttpGet] // GET: api/Products/GetAllProducts
        public async Task<IActionResult> GetAllProducts()
        {
            IList<GetAllProductsQueryResponse> response = await _mediator.Send(new GetAllProductsQueryRequest());
            return Ok(response);
        }

        [HttpGet] // GET: api/Products/GetLimitedProductsByPaging?{CurrentPage:int}&{PageSize:int}
        public async Task<IActionResult> GetLimitedProductsByPaging([FromQuery] GetLimitedProductsByPagingQueryRequest getLimitedProductsByPagingQueryRequest)
        {
            GetLimitedProductsByPagingQueryResponse response = await _mediator.Send(getLimitedProductsByPagingQueryRequest);
            return Ok(response);
        }

        [HttpGet("{Id:guid}")] // GET: api/Products/GetProductById/{Id:guid}
        public async Task<IActionResult> GetProductById([FromRoute] GetProductByIdQueryRequest getProductByIdQueryRequest)
        {
            GetProductByIdQueryResponse response = await _mediator.Send(getProductByIdQueryRequest);
            return Ok(response);
        }
    }
}
