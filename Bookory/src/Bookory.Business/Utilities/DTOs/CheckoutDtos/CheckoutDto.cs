namespace Bookory.Business.Utilities.DTOs.CheckoutDtos;

public record CheckoutDto(Guid PaymentID, decimal Amount, string? Currency = "USD");