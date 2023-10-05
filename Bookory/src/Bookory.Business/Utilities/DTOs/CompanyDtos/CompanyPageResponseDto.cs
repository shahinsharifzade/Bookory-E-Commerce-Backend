using Bookory.Business.Utilities.DTOs.BookDtos;

namespace Bookory.Business.Utilities.DTOs.CompanyDtos;

public record CompanyPageResponseDto(ICollection<CompanyGetResponseDto> Companies, decimal TotalCount);