using AutoMapper;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.CommentDtos;
using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.Exceptions.AuthException;
using Bookory.Business.Utilities.Exceptions.CommentExceptions;
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
    private readonly ICompanyService _companyService;
    private readonly IUserService _userService;
    private readonly IBookService _bookService;
    private readonly bool _isAuthenticated;
    private readonly IMapper _mapper;

    public CommentService(IMapper mapper, IHttpContextAccessor httpContextAccessor, ICommentRepository commentRepository, IUserService userService, IBookService bookService, ICompanyService companyService)
    {
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _commentRepository = commentRepository;
        _userService = userService;
        _isAuthenticated = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        _bookService = bookService;
        _companyService = companyService;
    }

    public async Task<ResponseDto> CreateCommentAsync(CommentPostDto commentPostDto)
    {
        if (!_isAuthenticated)
            throw new AuthenticationFailedException("Authentication required. Please log in.");

        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var comment = _mapper.Map<Comment>(commentPostDto);
        comment.UserId = userId;

        var book = await _bookService.GetBookAllDetailsByIdAsync(commentPostDto.EntityId);

        var previousSumRating = book.Rating * book.NumberOfRatings;
        book.Rating = (previousSumRating + comment.Rating) / (book.NumberOfRatings + 1);
        book.NumberOfRatings += 1;

        if (book.Company != null)
        {
            book.Company.Rating = book.Company.Books.Sum(a => a.Rating) / book.Company.Books.Count;
            await _companyService.ModifyCompanyAsync(book.Company);
        }

        await _commentRepository.CreateAsync(comment);
        await _commentRepository.SaveAsync();
        _bookService.UpdateBookByEntity(book);

        return new ResponseDto((int)HttpStatusCode.Created, "Comment successfully created");
    }

    public async Task<List<CommentGetResponseDto>> GetAllEntityCommentsAsync(Guid entityId, string entityType)
    {
        var comment = await _commentRepository.GetFiltered(comment => comment.EntityId == entityId && comment.EntityType == entityType, nameof(Comment.Replies)).ToListAsync();
        if (comment is null) throw new CommentNotFoundException("No comments found for the specified entity");

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
            throw new AuthenticationFailedException("Authentication required. Please log in.");

        var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var parentComment = await _commentRepository.GetByIdAsync(commentReplyPostDto.ParentId);

        if (parentComment is null)
            throw new ParentCommentNotFoundException("The parent comment with the specified ID could not be found.");

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

        return new ResponseDto((int)HttpStatusCode.Created, "Reply successfully posted");
    }
}
