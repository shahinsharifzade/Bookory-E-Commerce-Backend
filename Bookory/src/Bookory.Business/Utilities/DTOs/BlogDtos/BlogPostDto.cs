using Microsoft.AspNetCore.Http;

namespace Bookory.Business.Utilities.DTOs.BlogDtos;
public record BlogPostDto (string Title , string Content , IFormFile Image , List<Guid> CategoryIds);
