using AutoMapper;
using MediatR;
using TechECommerceServer.Application.Abstractions.Repositories.Product;
using TechECommerceServer.Application.Abstractions.Repositories.ProductImage;
using TechECommerceServer.Application.Abstractions.Storage;
using TechECommerceServer.Application.Bases;
using TechECommerceServer.Application.Features.Commands.Product.Rules;

namespace TechECommerceServer.Application.Features.Commands.ProductImage.UploadProductImage
{
    public class UploadProductImageCommandHandler : BaseHandler, IRequestHandler<UploadProductImageCommandRequest, Unit>
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductImageWriteRepository _productImageWriteRepository;
        private readonly IStorageService _storageService;
        private readonly BaseProductRules _productRules;
        public UploadProductImageCommandHandler(IMapper _mapper, IProductReadRepository productReadRepository, IProductImageWriteRepository productImageWriteRepository, IStorageService storageService, BaseProductRules productRules) : base(_mapper)
        {
            _productReadRepository = productReadRepository;
            _productImageWriteRepository = productImageWriteRepository;
            _storageService = storageService;
            _productRules = productRules;
        }

        public async Task<Unit> Handle(UploadProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Product product = await _productReadRepository.GetByIdAsync(request.Id);
            await _productRules.GivenProductMustBeLoad(request.Id, product);

            try
            {
                string path = Path.Combine("resource", "files", "product", "photo-images");
                List<(string fileName, string pathOrContainerName)> fileOperationResult = await _storageService.UploadFileAsync(path, request.Files);

                // note: iterate over the results and add each ProductImage entity one by one
                foreach (var file in fileOperationResult)
                {
                    Domain.Entities.ProductImage productImage = new Domain.Entities.ProductImage()
                    {
                        StorageType = _storageService.StorageName,
                        FileName = file.fileName,
                        Path = file.pathOrContainerName,
                        Products = new List<Domain.Entities.Product> { product }
                    };

                    bool isAdded = await _productImageWriteRepository.AddAsync(productImage);
                    if (!isAdded)
                    {
                        throw new InvalidOperationException($"Failed to add the product image for file {file.fileName}.");
                    }
                }

                await _productImageWriteRepository.SaveChangesAsync();

                return Unit.Value;
            }
            catch (Exception exc)
            {
                throw new InvalidOperationException("An error occurred while uploading product images.", exc);
            }
        }
    }
}
