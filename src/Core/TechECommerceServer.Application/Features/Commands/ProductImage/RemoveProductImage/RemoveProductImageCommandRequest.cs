using MediatR;
using Microsoft.AspNetCore.Mvc;
using TechECommerceServer.Application.Filters.Attributes;

namespace TechECommerceServer.Application.Features.Commands.ProductImage.RemoveProductImage
{
    public class RemoveProductImageCommandRequest : IRequest<Unit>
    {
        [FromRoute]
        public string Id { get; set; }

        [FromQuery]
        [GuidValidation]
        public string ImageId { get; set; }
    }
}
