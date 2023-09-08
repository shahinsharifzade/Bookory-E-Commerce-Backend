namespace Bookory.Business.Utilities.DTOs.PaymentDetailDto;

public record PaymentDetailGetResponseDto(Guid Id, decimal Amount, string? Status, string? TransactionId);