using Bookory.Business.Utilities.DTOs.CompanyDtos;
using FluentValidation;

namespace Bookory.Business.Utilities.Validators.CompanyValidators;

public class CompanyPostValidator : AbstractValidator<CompanyPostDto>
{
	public CompanyPostValidator()
	{

		RuleFor( c=> c.Username).NotEmpty().NotNull();
		RuleFor( c=> c.Name).NotEmpty().NotNull().MaximumLength(50);
		RuleFor( c=> c.Description).NotEmpty().NotNull();
		RuleFor( c=> c.Logo).NotEmpty().NotNull();
		RuleFor( c=> c.BannerImage).NotEmpty().NotNull();
		RuleFor( c=> c.ContactPhone).NotEmpty().NotNull();
		RuleFor( c=> c.ContactEmail).NotEmpty().NotNull();
		RuleFor( c=> c.Address).NotEmpty().NotNull();
    }
}
