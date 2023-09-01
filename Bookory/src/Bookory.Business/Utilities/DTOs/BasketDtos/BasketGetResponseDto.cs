using Bookory.Core.Models;

namespace Bookory.Business.Utilities.DTOs.BasketDtos;

public record BasketGetResponseDto(BasketItem BasketItem , ShoppingSession ShoppingSession , int Quantity);