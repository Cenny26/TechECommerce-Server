using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TechECommerceServer.Application.Features.Commands.Product.CreateProduct;
using TechECommerceServer.Application.Features.Commands.Product.RemoveProduct;
using TechECommerceServer.Application.Features.Commands.Product.UpdateProduct;
using TechECommerceServer.Application.Features.Commands.ProductImage.RemoveProductImage;
using TechECommerceServer.Application.Features.Commands.ProductImage.UploadProductImage;
using TechECommerceServer.Application.Features.Queries.Product.GetAllProducts;
using TechECommerceServer.Application.Features.Queries.Product.GetLimitedProductsByPaging;
using TechECommerceServer.Application.Features.Queries.Product.GetProductById;
using TechECommerceServer.Application.Features.Queries.ProductImage.GetMatchedImagesByProductId;

namespace TechECommerceServer.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
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

        [HttpDelete("{Id:guid}")] // DELETE: api/Products/RemoveProduct/{Id:guid}
        public async Task<IActionResult> RemoveProduct([FromRoute] RemoveProductCommandRequest removeProductCommandRequest)
        {
            await _mediator.Send(removeProductCommandRequest);
            return Ok((int)HttpStatusCode.Accepted);
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

        [HttpPost] // POST: api/Products/UploadProductImage?{Id:guid}
        public async Task<IActionResult> UploadProductImage([FromQuery] UploadProductImageCommandRequest uploadProductImageCommandRequest)
        {
            // todo: not to manually provide the files received during the [post] request!
            uploadProductImageCommandRequest.Files = HttpContext.Request.Form.Files;

            await _mediator.Send(uploadProductImageCommandRequest);
            return Ok((int)HttpStatusCode.Accepted);
        }

        [HttpDelete("{Id:guid}")] // DELETE: api/Products/RemoveProductImage/{Id:guid}?{imageId}
        public async Task<IActionResult> RemoveProductImage(RemoveProductImageCommandRequest removeProductImageCommandRequest)
        {
            await _mediator.Send(removeProductImageCommandRequest);
            return Ok((int)HttpStatusCode.Accepted);
        }

        [HttpGet("{Id:guid}")] // GET: api/Products/GetMatchedImagesByProductId/{Id:guid}
        public async Task<IActionResult> GetMatchedImagesByProductId([FromRoute] GetMatchedImagesByProductIdQueryRequest getMatchedImagesByProductIdQueryRequest)
        {
            List<GetMatchedImagesByProductIdQueryResponse>? response = await _mediator.Send(getMatchedImagesByProductIdQueryRequest);
            return Ok(response);
        }
    }
}
