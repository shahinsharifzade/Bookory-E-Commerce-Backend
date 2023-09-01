using Microsoft.AspNetCore.Http;

namespace Bookory.Business.Utilities.DTOs.BookDtos;
public record BookPutDto(Guid Id, string Title, List<IFormFile>? Images, int MainImageIndex, string Description, decimal Price, decimal DiscountPrice, Guid AuthorId);
