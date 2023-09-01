using Bookory.Business.Utilities.DTOs.AuthDtos;
using FluentValidation;
namespace Bookory.Business.Utilities.Validators.AuthValidators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
	public LoginDtoValidator()
	{
		RuleFor(l => l.UserName).NotEmpty().NotNull();
        RuleFor(l => l.Password).NotEmpty().NotNull();
    }
}
