using Bookory.Business.Utilities.DTOs.BookDtos;

namespace Bookory.Business.Utilities.DTOs.BasketDtos;

public record BasketGetResponseDto(Guid Id, BookGetResponseDto BasketBook , int Quantity, decimal Price);