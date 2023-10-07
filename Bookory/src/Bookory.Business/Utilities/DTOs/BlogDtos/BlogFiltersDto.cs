namespace Bookory.Business.Utilities.DTOs.BlogDtos;

public record BlogFiltersDto(List<Guid>? Categories, string? Search , string? SortBy);