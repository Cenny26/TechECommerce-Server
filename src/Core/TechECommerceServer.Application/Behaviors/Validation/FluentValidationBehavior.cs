using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace TechECommerceServer.Application.Behaviors.Validation
{
    public class FluentValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public FluentValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            ValidationContext<TRequest> context = new ValidationContext<TRequest>(request);
            List<ValidationFailure> failtures = _validators
                .Select(validator => validator.Validate(context))
                .SelectMany(result => result.Errors)
                .GroupBy(element => element.ErrorMessage)
                .Select(elemet => elemet.First())
                .Where(field => field is not null)
                .ToList();

            if (failtures.Any())
                throw new ValidationException(failtures);

            return await next();
        }
    }
}
