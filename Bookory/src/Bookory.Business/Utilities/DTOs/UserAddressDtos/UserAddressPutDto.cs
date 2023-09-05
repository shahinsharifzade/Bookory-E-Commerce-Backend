namespace Bookory.Business.Utilities.DTOs.UserAddressDtos;

public record UserAddressPutDto(Guid Id, string AddressLine1, string? AddressLine2, string City, string PostalCode, string Country, string? Telephone, string Mobile);
