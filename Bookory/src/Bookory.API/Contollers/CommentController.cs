using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.CommentDtos;
using Microsoft.AspNetCore.Mvc;

namespace Bookory.API.Contollers;

[Route("api/[controller]")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet("get/{entityId}/{entityType}")]
    public async Task<IActionResult> Get(Guid entityId, string entityType)
    {
        var comments = await _commentService.GetAllEntityCommentsAsync(entityId, entityType);

        return Ok(comments);
    }

    [HttpPost("reply")]
    public async Task<IActionResult> ReplyToComment([FromBody] CommentReplyPostDto commentReplyPostDto)
    {
        var response = await _commentService.ReplyToCommentAsync(commentReplyPostDto);
        return Ok(response);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Post([FromBody] CommentPostDto commentPostDto)
    {
        var response = await _commentService.CreateCommentAsync(commentPostDto);
        return Ok(response);
    }
}
