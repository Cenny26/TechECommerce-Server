using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TechECommerceServer.Application.Abstractions.Repositories.File;
using TechECommerceServer.Application.Abstractions.Repositories.Product;
using TechECommerceServer.Application.Abstractions.Repositories.ProductImage;
using TechECommerceServer.Application.Abstractions.Storage;
using TechECommerceServer.Application.Bases;
using TechECommerceServer.Application.Features.Commands.Product.Rules;

namespace TechECommerceServer.Application.Features.Commands.ProductImage.RemoveProductImage
{
    public class RemoveProductImageCommandHandler : BaseHandler, IRequestHandler<RemoveProductImageCommandRequest, Unit>
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IStorageService _storageService;
        private readonly IFileWriteRepository _fileWriteRepository;
        private readonly IProductImageWriteRepository _productImageWriteRepository;
        private readonly BaseProductRules _productRules;
        public RemoveProductImageCommandHandler(IMapper _mapper, IProductReadRepository productReadRepository, IStorageService storageService, IFileWriteRepository fileWriteRepository, IProductImageWriteRepository productImageWriteRepository, BaseProductRules productRules) : base(_mapper)
        {
            _productReadRepository = productReadRepository;
            _storageService = storageService;
            _fileWriteRepository = fileWriteRepository;
            _productImageWriteRepository = productImageWriteRepository;
            _productRules = productRules;
        }

        public async Task<Unit> Handle(RemoveProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Domain.Entities.Product? product = await _productReadRepository.Table
                    .Include(navigationPropertyPath => navigationPropertyPath.ProductImages)
                    .FirstOrDefaultAsync(item => item.Id == Guid.Parse(request.Id));

                Domain.Entities.ProductImage productImage = product.ProductImages.FirstOrDefault(item => item.Id == Guid.Parse(request.ImageId));

                if (productImage is not null)
                {
                    product.ProductImages.Remove(productImage);

                    await _productImageWriteRepository.SaveChangesAsync();

                    await _fileWriteRepository.RemoveByIdAsync(Convert.ToString(productImage.Id));
                    await _fileWriteRepository.SaveChangesAsync();

                    await _storageService.DeleteFileAsync(productImage.Path, productImage.FileName);
                }

                return Unit.Value;
            }
            catch (ArgumentNullException exc)
            {
                throw new ArgumentNullException(nameof(Domain.Entities.Product), $"The product with the given identity: '{request.Id}' was not found, check it out!");
            }
            catch (Exception exc)
            {
                // todo: log the exception and possibly wrap it in a custom exception!
                throw new ApplicationException("An error occurred while removing the product image.", exc);
            }
        }
    }
}
