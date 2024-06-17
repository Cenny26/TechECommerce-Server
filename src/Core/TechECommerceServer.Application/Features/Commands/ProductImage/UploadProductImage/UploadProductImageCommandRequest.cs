using MediatR;
using Microsoft.AspNetCore.Http;

namespace TechECommerceServer.Application.Features.Commands.ProductImage.UploadProductImage
{
    public class UploadProductImageCommandRequest : IRequest<Unit>
    {
        public string Id { get; set; }
        public IFormFileCollection? Files { get; set; }
    }
}
