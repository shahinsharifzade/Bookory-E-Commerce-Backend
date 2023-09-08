using AutoMapper;
using Bookory.Business.Services.Interfaces;
using Bookory.Business.Utilities.DTOs.OrderDetailDtos;
using Bookory.Core.Models;
using Bookory.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookory.Business.Services.Implementations;

public class OrderDetailService : IOrderDetailService
{
    private readonly IMapper _mapper;
    private readonly IOrderDetailRepository _orderDetailRepository;

    public OrderDetailService(IMapper mapper, IOrderDetailRepository orderDetailRepository)
    {
        _mapper = mapper;
        _orderDetailRepository = orderDetailRepository;
    }


    public async Task<List<OrderDetailGetResponseDto>> GetAllOrderDetailAsync()
    {
        var orderDetail =await _orderDetailRepository.GetAll(
            nameof(OrderDetail.OrderItems),
            $"{nameof(OrderDetail.OrderItems)}.{nameof(OrderItem.Book)}",
            $"{nameof(OrderDetail.OrderItems)}.{nameof(OrderItem.Book)}.{nameof(Book.BookGenres)}.{nameof(BookGenre.Genre)}",
            $"{nameof(OrderDetail.OrderItems)}.{nameof(OrderItem.Book)}.{nameof(Book.Images)}",
            $"{nameof(OrderDetail.OrderItems)}.{nameof(OrderItem.Book)}.{nameof(Book.Author)}",
            $"{nameof(OrderDetail.OrderItems)}.{nameof(OrderItem.Book)}.{nameof(Book.Author)}.{nameof(Author.Images)}").ToListAsync();

        var orderDetailsDto = _mapper.Map<List<OrderDetailGetResponseDto>>(orderDetail);
        return orderDetailsDto;
    }

    public async Task<OrderDetailGetResponseDto> GetOrderDetailAsync(Guid id)
    {
        var orderDetail = await _orderDetailRepository.GetByIdAsync(id,
            nameof(OrderDetail.OrderItems),
            $"{nameof(OrderDetail.OrderItems)}.{nameof(OrderItem.Book)}",
            $"{nameof(OrderDetail.OrderItems)}.{nameof(OrderItem.Book)}.{nameof(Book.BookGenres)}.{nameof(BookGenre.Genre)}",
            $"{nameof(OrderDetail.OrderItems)}.{nameof(OrderItem.Book)}.{nameof(Book.Images)}",
            $"{nameof(OrderDetail.OrderItems)}.{nameof(OrderItem.Book)}.{nameof(Book.Author)}",
            $"{nameof(OrderDetail.OrderItems)}.{nameof(OrderItem.Book)}.{nameof(Book.Author)}.{nameof(Author.Images)}");

        var orderDetailDto = _mapper.Map<OrderDetailGetResponseDto>(orderDetail);
        return orderDetailDto;
    }

    public async Task<List<OrderDetailGetResponseDto>> GetAllOrderDetailByUserIdAsync(string id)
    {
        var orderDetail =await _orderDetailRepository.GetFiltered(od => od.UserId == id,
            nameof(OrderDetail.OrderItems),
            $"{nameof(OrderDetail.OrderItems)}.{nameof(OrderItem.Book)}",
            $"{nameof(OrderDetail.OrderItems)}.{nameof(OrderItem.Book)}.{nameof(Book.BookGenres)}.{nameof(BookGenre.Genre)}",
            $"{nameof(OrderDetail.OrderItems)}.{nameof(OrderItem.Book)}.{nameof(Book.Images)}",
            $"{nameof(OrderDetail.OrderItems)}.{nameof(OrderItem.Book)}.{nameof(Book.Author)}",
            $"{nameof(OrderDetail.OrderItems)}.{nameof(OrderItem.Book)}.{nameof(Book.Author)}.{nameof(Author.Images)}").ToListAsync();

        var orderDetailsDto = _mapper.Map<List<OrderDetailGetResponseDto>>(orderDetail);
        return orderDetailsDto;
    }

    public async Task<OrderDetailGetResponseDto> CreateOrderDetailAsync(OrderDetailPostDto orderDetailPostDto)
    {
        var newOrderDetail = _mapper.Map<OrderDetail>(orderDetailPostDto);

        await _orderDetailRepository.CreateAsync(newOrderDetail);
        await _orderDetailRepository.SaveAsync();

        var orderDetailDto = _mapper.Map<OrderDetailGetResponseDto>(newOrderDetail);
        return orderDetailDto;
    }

    
}
