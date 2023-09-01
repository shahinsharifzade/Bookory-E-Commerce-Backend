using Bookory.Business.Utilities.DTOs.GenreDtos;
using FluentValidation;

namespace Bookory.Business.Utilities.Validators.GenreValidaors;

public class GenrePutValidator : AbstractValidator<GenrePutDto>
{
    public GenrePutValidator()
    {
        RuleFor(g => g.Name).NotNull().NotEmpty();
        RuleFor(g => g.Id).NotEmpty().NotNull();

    }
}
