namespace Bookory.Business.Utilities.DTOs.Stripe;

public record ChargeResourcePostDto(string Currency, long Amount, string CustomerId, string ReceiptEmail, string Description);
