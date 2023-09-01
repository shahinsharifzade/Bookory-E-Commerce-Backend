using Bookory.Business.Utilities.DTOs.BookDtos;
using FluentValidation;

namespace Bookory.Business.Utilities.Validators.BookValidators;

public class BookPutDtoValidator : AbstractValidator<BookPutDto>
{
    public BookPutDtoValidator()
    {
        RuleFor(b => b.Title).NotEmpty().NotNull().MaximumLength(300);
        RuleFor(b => b.Images).NotEmpty().NotNull();
        RuleFor(b => b.Description).NotEmpty().NotNull().MaximumLength(300);
        RuleFor(b => b.Price).NotEmpty().NotNull();
        RuleFor(a => a.MainImageIndex).GreaterThanOrEqualTo(0);

        RuleFor(g => g.Id).NotEmpty().NotNull();
        RuleFor(b => b.AuthorId).NotEmpty().NotNull();
    }
}
