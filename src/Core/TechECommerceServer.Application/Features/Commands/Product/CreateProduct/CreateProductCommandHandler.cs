using AutoMapper;
using MediatR;
using TechECommerceServer.Application.Abstractions.Repositories.Product;

namespace TechECommerceServer.Application.Features.Commands.Product.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, Unit>
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IMapper _mapper;
        public CreateProductCommandHandler(IProductWriteRepository productWriteRepository, IMapper mapper)
        {
            _productWriteRepository = productWriteRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Product product = _mapper.Map<Domain.Entities.Product>(request);

            bool result = await _productWriteRepository.AddAsync(entity: product);
            if (result)
                await _productWriteRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
