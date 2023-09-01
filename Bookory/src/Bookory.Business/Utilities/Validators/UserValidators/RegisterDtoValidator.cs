using Bookory.Business.Utilities.DTOs.UserDtos;
using FluentValidation;
namespace Bookory.Business.Utilities.Validators.UserValidators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
	public RegisterDtoValidator()
	{
        RuleFor(dto => dto.UserName).NotEmpty();
        RuleFor(dto => dto.FullName).NotEmpty();
        RuleFor(dto => dto.Email).NotEmpty().EmailAddress();
        RuleFor(dto => dto.Password).NotEmpty();
        RuleFor(dto => dto.PasswordConfirm).NotEmpty()
            .Equal(dto => dto.Password).WithMessage("Passwords do not match.");
    }
}
