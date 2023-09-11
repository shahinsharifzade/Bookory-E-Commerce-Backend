using Bookory.Business.Utilities.DTOs.CommentDtos;
using Bookory.Business.Utilities.DTOs.Common;

namespace Bookory.Business.Services.Interfaces;

public interface ICommentService
{
    Task<List<CommentGetResponseDto>> GetAllEntityCommentsAsync(Guid entityId, string entityType);
    Task<ResponseDto> CreateCommentAsync(CommentPostDto commentPostDto);
    Task<ResponseDto> ReplyToCommentAsync(CommentReplyPostDto commentReplyPostDto);
}
