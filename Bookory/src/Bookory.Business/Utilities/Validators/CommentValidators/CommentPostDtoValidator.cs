using Bookory.Business.Utilities.DTOs.CommentDtos;
using FluentValidation;

namespace Bookory.Business.Utilities.Validators.CommentValidators;

public class CommentPostDtoValidator : AbstractValidator<CommentPostDto>
{
	public CommentPostDtoValidator()
	{
		RuleFor(c => c.EntityType).NotEmpty().NotNull();
		RuleFor(c => c.EntityId).NotEmpty().NotNull();
		RuleFor(c => c.Content).NotEmpty().NotNull();
	}
}
