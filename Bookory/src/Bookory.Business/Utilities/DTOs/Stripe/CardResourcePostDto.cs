namespace Bookory.Business.Utilities.DTOs.Stripe;

public record CardResourcePostDto(string Name, string Number, string ExpiryYear, string ExpiryMonth, string Cvc);
