using Bookory.Business.Utilities.DTOs.OrderItemDtos;

namespace Bookory.Business.Utilities.DTOs.OrderDetailDtos;

public record OrderDetailGetResponseDto(Guid Id, decimal Total, string? UserId, ICollection<OrderItemGetResponseDto>? OrderItems );