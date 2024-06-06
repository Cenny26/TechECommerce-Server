﻿using AutoMapper;
using MediatR;
using TechECommerceServer.Application.Abstractions.Repositories.Product;
using TechECommerceServer.Application.Bases;
using TechECommerceServer.Application.Features.Commands.Product.Rules;

namespace TechECommerceServer.Application.Features.Commands.Product.CreateProduct
{
    public class CreateProductCommandHandler : BaseHandler, IRequestHandler<CreateProductCommandRequest, Unit>
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly BaseProductRules _productRules;
        public CreateProductCommandHandler(IProductWriteRepository productWriteRepository, BaseProductRules productRules, IMapper _mapper) : base(_mapper)
        {
            _productWriteRepository = productWriteRepository;
            _productRules = productRules;
        }

        public async Task<Unit> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            await _productRules.ProductPriceAndDiscountValuesMustBeValid(request.Price, request.Discount);

            Domain.Entities.Product product = _mapper.Map<Domain.Entities.Product>(request);

            bool result = await _productWriteRepository.AddAsync(entity: product);
            if (result)
                await _productWriteRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
