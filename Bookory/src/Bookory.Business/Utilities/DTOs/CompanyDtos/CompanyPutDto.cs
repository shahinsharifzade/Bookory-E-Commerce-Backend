using Microsoft.AspNetCore.Http;

namespace Bookory.Business.Utilities.DTOs.CompanyDtos;

public record CompanyPutDto(Guid Id, string Name, string Description, IFormFile Logo, IFormFile BannerImage, string ContactEmail, string ContactPhone, string? Address);