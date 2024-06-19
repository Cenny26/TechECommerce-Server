using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TechECommerceServer.Application.Abstractions.Repositories.File;
using TechECommerceServer.Application.Abstractions.Repositories.Product;
using TechECommerceServer.Application.Abstractions.Repositories.ProductImage;
using TechECommerceServer.Application.Abstractions.Storage;
using TechECommerceServer.Application.Bases;
using TechECommerceServer.Application.Features.Commands.Product.Rules;

namespace TechECommerceServer.Application.Features.Commands.Product.RemoveProduct
{
    public class RemoveProductCommandHandler : BaseHandler, IRequestHandler<RemoveProductCommandRequest, Unit>
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IStorageService _storageService;
        private readonly IFileWriteRepository _fileWriteRepository;
        private readonly IProductImageWriteRepository _productImageWriteRepository;
        private readonly BaseProductRules _productRules;
        public RemoveProductCommandHandler(IMapper _mapper, IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IStorageService storageService, IFileWriteRepository fileWriteRepository, IProductImageWriteRepository productImageWriteRepository, BaseProductRules productRules) : base(_mapper)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _storageService = storageService;
            _fileWriteRepository = fileWriteRepository;
            _productImageWriteRepository = productImageWriteRepository;
            _productRules = productRules;
        }

        public async Task<Unit> Handle(RemoveProductCommandRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Product? product = await _productReadRepository.Table.Include(navigationPropertyPath => navigationPropertyPath.ProductImages).FirstOrDefaultAsync(item => item.Id == Guid.Parse(request.Id));
            await _productRules.GivenProductMustBeLoad(request.Id, product);
            List<Domain.Entities.ProductImage>? productImages = product?.ProductImages.ToList();

            // note: firstly, deleting all file(s) related to the current product.
            if (productImages.Any())
            {
                foreach (Domain.Entities.ProductImage image in productImages)
                {
                    product?.ProductImages.Remove(image);
                    await _productImageWriteRepository.SaveChangesAsync();

                    await _fileWriteRepository.RemoveByIdAsync(Convert.ToString(image.Id));
                    await _fileWriteRepository.SaveChangesAsync();

                    await _storageService.DeleteFileAsync(image.Path, image.FileName);
                }
            }

            // note: deleting a product entity.
            await _productWriteRepository.RemoveByIdAsync(request.Id);
            await _productWriteRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}