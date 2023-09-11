namespace Bookory.Business.Utilities.DTOs.CommentDtos;

public record CommentReplyPostDto(Guid ParentId, string Content, string EntityType);    