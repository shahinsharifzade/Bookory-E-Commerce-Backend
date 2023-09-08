using Bookory.Business.Utilities.DTOs.OrderDetailDtos;

namespace Bookory.Business.Services.Interfaces;

public interface IOrderDetailService
{
    Task<List<OrderDetailGetResponseDto>> GetAllOrderDetailAsync();
    Task<List<OrderDetailGetResponseDto>> GetAllOrderDetailByUserIdAsync(string id);
    Task<OrderDetailGetResponseDto> GetOrderDetailAsync(Guid id);
    Task<OrderDetailGetResponseDto> CreateOrderDetailAsync(OrderDetailPostDto orderDetailPostDto);

}
