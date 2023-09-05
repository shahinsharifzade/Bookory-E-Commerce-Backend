using Bookory.Business.Utilities.DTOs.BasketDtos;
using FluentValidation;

namespace Bookory.Business.Utilities.Validators.BasketValidators;

public class BasketPutDtoValidator : AbstractValidator<BasketPutDto>
{
	public BasketPutDtoValidator()
	{
		RuleFor( b => b.Id).NotNull().NotEmpty();
        RuleFor(b => b.Quantity).GreaterThan(0);
    }
}
