using Bookory.Business.Utilities.DTOs.BookDtos;

namespace Bookory.Business.Utilities.DTOs.OrderItemDtos;

public record OrderItemGetResponseDto(Guid Id, int Quantity, BookGetResponseDto Book, Guid OrderDetailId);
