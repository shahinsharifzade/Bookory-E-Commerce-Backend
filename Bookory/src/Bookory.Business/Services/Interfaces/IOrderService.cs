using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.DTOs.OrderDetailDtos;
using Bookory.Business.Utilities.DTOs.OrderDtos;

namespace Bookory.Business.Services.Interfaces;

public interface IOrderService
{
    public Task<ResponseDto> PurchaseBooks(OrderPostDto orderPostDto);

}
