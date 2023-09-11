using Bookory.Business.Utilities.DTOs.UserAddressDtos;
using FluentValidation;
namespace Bookory.Business.Utilities.Validators.UserAddressValidators;

public class AddressPostDtoValidator : AbstractValidator<UserAddressPostDto>
{
    public AddressPostDtoValidator()
    {
        RuleFor(ua => ua.AddressLine1).NotEmpty().NotNull().MaximumLength(100);
        RuleFor(ua => ua.City).NotEmpty().NotNull().MaximumLength(50);
        RuleFor(ua => ua.PostalCode).NotEmpty().NotNull().MaximumLength(50);
        RuleFor(ua => ua.Country).NotEmpty().NotNull().MaximumLength(100);
        RuleFor(ua => ua.Mobile).NotEmpty().NotNull().MaximumLength(100);
    }
}
