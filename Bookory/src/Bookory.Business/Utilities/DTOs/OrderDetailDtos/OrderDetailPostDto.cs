namespace Bookory.Business.Utilities.DTOs.OrderDetailDtos;

public record OrderDetailPostDto(decimal Total, string? UserId, Guid PaymentDetailId);
