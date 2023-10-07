using Bookory.Business.Utilities.DTOs.BlogDtos;
using Bookory.Business.Utilities.DTOs.BookDtos;
using Bookory.Business.Utilities.DTOs.Common;

namespace Bookory.Business.Services.Interfaces;

public interface IBlogService
{
    Task<List<BlogGetResponseDto>> GetAllBlogsAsync(string? search);
    Task<BlogGetResponseDto> GetBlogByIdAsync(Guid id);
    Task<BlogPageResponseDto> GetFilteredBlogsAsync(int pageNumber, int pageSize, BlogFiltersDto filters);
    Task<ResponseDto> CreateBlogAsync(BlogPostDto blogPostDto);
    Task<ResponseDto> UpdateBlogAsync(BlogPutDto blogPutDto);
    Task<ResponseDto> DeleteBlogAsync(Guid id);
    
}
