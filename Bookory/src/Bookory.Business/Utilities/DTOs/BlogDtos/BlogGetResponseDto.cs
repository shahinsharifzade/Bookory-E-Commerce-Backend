using Bookory.Core.Models;

namespace Bookory.Business.Utilities.DTOs.BlogDtos;

public record BlogGetResponseDto(Guid Id, string Title, string Content, string Image, DateTime CreatedAt, string? CreateBy, ICollection<Category> Categories);