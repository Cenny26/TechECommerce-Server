using FluentValidation;

namespace TechECommerceServer.Application.Features.Commands.Product.CreateProduct
{
    public class CreateProductCommandValidatior : AbstractValidator<CreateProductCommandRequest>
    {
        public CreateProductCommandValidatior()
        {
            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("Please do not leave the product name blank.")
                .MaximumLength(100).WithMessage("Please enter a product name with maximum 100 characters.")
                .WithName("product's title");

            RuleFor(p => p.Description)
                .MaximumLength(255).WithMessage("Please enter a product description with maximum 255 characters.")
                .WithName("product's description");

            RuleFor(p => p.StockQuantity)
                .NotEmpty().WithMessage("Please do not leave stock quantity blank.")
                .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative.")
                .WithName("product's stock quantity");

            RuleFor(p => p.Price)
                .NotEmpty().WithMessage("Please do not leave price blank.")
                .GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative.")
                .WithName("product's price");

            RuleFor(p => p.Discount)
                .NotNull().WithMessage("Please provide a discount value.")
                .InclusiveBetween(0, 100)
                    .WithMessage("Discount must be between 0 and 100.")
                .WithName("product's current discount");
        }
    }
}
