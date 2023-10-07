using Bookory.Business.Utilities.DTOs.BookDtos;

namespace Bookory.Business.Utilities.DTOs.BlogDtos;
public record BlogPageResponseDto(ICollection<BlogGetResponseDto> Blogs, decimal TotalCount);