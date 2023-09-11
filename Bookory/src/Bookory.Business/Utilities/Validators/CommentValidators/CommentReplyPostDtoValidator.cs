using Bookory.Business.Utilities.DTOs.CommentDtos;
using FluentValidation;

namespace Bookory.Business.Utilities.Validators.CommentValidators;

public class CommentReplyPostDtoValidator : AbstractValidator<CommentReplyPostDto>
{
	public CommentReplyPostDtoValidator()
	{
		RuleFor(c => c.Content).NotNull().NotEmpty();
		RuleFor(c => c.ParentId).NotNull().NotEmpty();
		RuleFor(c => c.EntityType).NotNull().NotEmpty();
    }
}
