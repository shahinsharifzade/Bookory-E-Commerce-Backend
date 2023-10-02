using Bookory.Business.Utilities.DTOs.UserDtos;

namespace Bookory.Business.Utilities.DTOs.CompanyDtos;

public record CompanyGetResponseDtoInclude(Guid Id, string UserId, string Name, string Description, string Logo, string BannerImage, string ContactEmail, string ContactPhone, string? Address, decimal? Rating, UserGetResponseDto User);