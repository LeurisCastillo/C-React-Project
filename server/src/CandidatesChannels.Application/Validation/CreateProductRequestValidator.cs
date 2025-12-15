using CandidatesChannels.Application.DTOs.Products;
using FluentValidation;

namespace CandidatesChannels.Application.Validation;

public sealed class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Category).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Stock).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Description).MaximumLength(2000);
    }
}
