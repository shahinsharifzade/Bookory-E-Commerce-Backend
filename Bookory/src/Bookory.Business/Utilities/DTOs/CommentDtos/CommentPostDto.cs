namespace Bookory.Business.Utilities.DTOs.CommentDtos;

public record CommentPostDto(Guid EntityId, string EntityType, int Rating, string Content);