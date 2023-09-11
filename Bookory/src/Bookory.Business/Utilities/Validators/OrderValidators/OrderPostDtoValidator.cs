using Bookory.Business.Utilities.DTOs.OrderDtos;
using FluentValidation;

namespace Bookory.Business.Utilities.Validators.OrderValidators;

public class OrderPostDtoValidator : AbstractValidator<OrderPostDto>
{
	public OrderPostDtoValidator()
	{
		RuleFor( o => o.StripeToken).NotNull().NotEmpty();
		RuleFor( o => o.AddressId).NotNull().NotEmpty();
		RuleFor( o => o.StripeEmail).NotNull().NotEmpty();
    }
}
