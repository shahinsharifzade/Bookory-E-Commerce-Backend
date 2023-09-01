using Microsoft.AspNetCore.Http;
namespace Bookory.Business.Utilities.DTOs.AuthorDtos;

public record AuthorPutDto(Guid Id, string Name, List<IFormFile>? Images, int MainImageIndex, string Biography);
