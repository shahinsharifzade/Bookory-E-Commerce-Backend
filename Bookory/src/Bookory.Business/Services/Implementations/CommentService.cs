using AutoMapper;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.CommentDtos;
using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Core.Models;
using Bookory.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace Bookory.Business.Services.Implementations;

public class CommentService : ICommentService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICommentRepository _commentRepository;
    private readonly IUserService _userService;
    private readonly bool _isAuthenticated;
    private readonly IMapper _mapper;

    public CommentService(IMapper mapper, IHttpContextAccessor httpContextAccessor, ICommentRepository commentRepository, IUserService userService)
    {
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _commentRepository = commentRepository;
        _userService = userService;
        _isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
    }

    public async Task<ResponseDto> CreateCommentAsync(CommentPostDto commentPostDto)
    {
        if (!_isAuthenticated)
            throw new Exception("Please Login");

        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var comment = _mapper.Map<Comment>(commentPostDto);
        comment.UserId = userId;

        await _commentRepository.CreateAsync(comment);
        await _commentRepository.SaveAsync();

        return new ResponseDto((int)HttpStatusCode.Created, "Success");
    }

    public async Task<List<CommentGetResponseDto>> GetAllEntityCommentsAsync(Guid entityId, string entityType)
    {
        if (!_isAuthenticated)
            throw new Exception("Please Login");

        var comment = await _commentRepository.GetFiltered(comment => comment.EntityId == entityId && comment.EntityType == entityType, nameof(Comment.Replies)).ToListAsync();
        if (comment is null) throw new Exception("Null");

        var commentDto = _mapper.Map<List<CommentGetResponseDto>>(comment);

        foreach (var commentItem in commentDto) // Adding All Reply's comments
            commentItem.Replies = GetRepliesRecursive(commentItem.Id);

        return commentDto;
    }

    private List<CommentGetResponseDto> GetRepliesRecursive(Guid parentId)
    {
        var replies = new List<CommentGetResponseDto>();

        // Check any comment exist which Reference Id equal to ParentId (Reply's id)
        var parentComment = _commentRepository.GetFiltered(comment => comment.RefId == parentId, nameof(Comment.Replies)).ToList(); 

        if (parentComment != null && parentComment.Count > 0)
        {
            foreach (var comment in parentComment)
            {
                var commentDto = _mapper.Map<CommentGetResponseDto>(comment);
                commentDto.Replies = GetRepliesRecursive(comment.Id);
                replies.Add(commentDto);
            }
        }

        return replies;
    }

    public async Task<ResponseDto> ReplyToCommentAsync(CommentReplyPostDto commentReplyPostDto)
    {
        if (!_isAuthenticated)
            throw new Exception("Please Login");

        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var parentComment = await _commentRepository.GetByIdAsync(commentReplyPostDto.ParentId);

        if (parentComment is null)
            throw new Exception("Parent Comment can not found");

        var replyComment = new Comment
        {
            UserId = userId,
            Content = commentReplyPostDto.Content,
            RefId = commentReplyPostDto.ParentId,
            EntityType = commentReplyPostDto.EntityType
        };

        parentComment.Replies.Add(replyComment);
        _commentRepository.Update(parentComment);
        await _commentRepository.SaveAsync();

        return new ResponseDto((int)HttpStatusCode.Created, "Success");
    }
}
