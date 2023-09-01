using Bookory.Business.Utilities.DTOs.GenreDtos;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Bookory.Business.Utilities.Validators.GenreValidaors;

public class GenrePostValidator : AbstractValidator<GenrePostDto>
{
    public GenrePostValidator()
    {
        RuleFor(g => g.Name).NotEmpty().NotNull().WithMessage("BOsh ola bilmezz");

    }
}
