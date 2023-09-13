using Microsoft.AspNetCore.Http;

namespace Bookory.Business.Utilities.DTOs.CompanyDtos;

public record CompanyPostDto (string Username, string Name, string Description, IFormFile Logo, IFormFile BannerImage, string ContactEmail, string ContactPhone, string? Address );


