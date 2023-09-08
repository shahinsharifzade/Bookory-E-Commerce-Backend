namespace Bookory.Business.Utilities.DTOs.Stripe;

public record CustomerResourcePostDto(string Email, string Name, CardResourcePostDto Card);