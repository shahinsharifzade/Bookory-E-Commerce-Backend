using AutoMapper;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.Common;
using Bookory.Business.Utilities.DTOs.OrderItemDtos;
using Bookory.Core.Models;
using Bookory.DataAccess.Repositories.Interfaces;
using System.Net;
using System.Security.AccessControl;

namespace Bookory.Business.Services.Implementations;

public class OrderItemService : IOrderItemService
{
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IMapper _mapper;
    public OrderItemService(IOrderItemRepository orderItemRepository, IMapper mapper)
    {
        _orderItemRepository = orderItemRepository;
        _mapper = mapper;
    }

    public async Task<ResponseDto> CreateOrderItemAsync(OrderItemPostDto orderItemPostDto)
    {
        var orderItem = _mapper.Map<OrderItem>(orderItemPostDto);

        await _orderItemRepository.CreateAsync(orderItem);
        await _orderItemRepository.SaveAsync();

        return new ResponseDto((int)HttpStatusCode.Created, "Order Item successfully created");
    }
}
