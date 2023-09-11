using Bookory.Business.Utilities.DTOs.AuthorDtos;
using FluentValidation;

namespace Bookory.Business.Utilities.Validators.AuthorValidator;

public class AuthorPutDtoValidator : AbstractValidator<AuthorPutDto>
{
    public AuthorPutDtoValidator()
    {
        RuleFor(a => a.Id).NotEmpty().NotNull();

        RuleFor(a => a.Name).NotEmpty().NotNull().MaximumLength(100);
        RuleFor(a => a.Biography).NotEmpty().NotNull().MaximumLength(1000);
        RuleFor(a => a.Images).NotEmpty().NotNull();
        RuleFor(a => a.MainImageIndex).GreaterThanOrEqualTo(0);
    }
}
