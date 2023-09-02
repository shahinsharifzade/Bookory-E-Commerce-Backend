using Bookory.Business.Utilities.DTOs.BookDtos;
using Bookory.Core.Models;

namespace Bookory.Business.Utilities.DTOs.BasketDtos;

public record BasketGetResponseDto(BookGetResponseDto BasketBook , int Quantity);