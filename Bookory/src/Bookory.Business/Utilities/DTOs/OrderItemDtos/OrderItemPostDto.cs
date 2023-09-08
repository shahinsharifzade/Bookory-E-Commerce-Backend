namespace Bookory.Business.Utilities.DTOs.OrderItemDtos;

public record OrderItemPostDto(int Quantity, Guid BookId, Guid OrderDetailId);