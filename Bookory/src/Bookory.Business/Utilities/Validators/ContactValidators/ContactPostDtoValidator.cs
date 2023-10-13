using Bookory.Business.Utilities.DTOs.ContactDtos;
using Bookory.Core.Models;
using FluentValidation;

namespace Bookory.Business.Utilities.Validators.ContactValidators;

public class ContactPostDtoValidator : AbstractValidator<ContactPostDto>
{
	public ContactPostDtoValidator()
	{
		RuleFor(  c => c.Name).NotEmpty().NotNull();
		RuleFor(  c => c.Email).NotEmpty().NotNull().EmailAddress();
		RuleFor(  c => c.Message).NotEmpty().NotNull().MaximumLength(300);

    }
}
