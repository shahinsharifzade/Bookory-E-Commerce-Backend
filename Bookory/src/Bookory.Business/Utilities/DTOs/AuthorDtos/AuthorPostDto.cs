using Microsoft.AspNetCore.Http;
namespace Bookory.Business.Utilities.DTOs.AuthorDtos;

public record AuthorPostDto(string Name, List<IFormFile> Images, string Biography, int MainImageIndex);
