using Bookory.Business.Utilities.DTOs.WishlistDtos;
using FluentValidation;

namespace Bookory.Business.Utilities.Validators.WishlistValidators;

public class WishlistPostDtoValidator : AbstractValidator<WishlistPostDto>
{
	public WishlistPostDtoValidator()
	{
		RuleFor(w => w.Id).NotEmpty().NotNull();
	}
}
