using Bookory.Core.Models;
namespace Bookory.Business.Utilities.DTOs.ShoppingSessionDtos;

public record ShoppingSessionGetResponseDto(decimal TotalPrice, bool IsOrdered, string UserId, ICollection<BasketItem> BasketItems);