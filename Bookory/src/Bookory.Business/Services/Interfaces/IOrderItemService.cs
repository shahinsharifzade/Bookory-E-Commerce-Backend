using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.DTOs.OrderItemDtos;

namespace Bookory.Business.Services.Interfaces;

public interface IOrderItemService
{
    Task<ResponseDto> CreateOrderItemAsync(OrderItemPostDto orderItemPostDto);
}
