namespace Bookory.Business.Utilities.DTOs.OrderDtos;

public record OrderPostDto(string StripeEmail, string StripeToken, Guid AddressId);
