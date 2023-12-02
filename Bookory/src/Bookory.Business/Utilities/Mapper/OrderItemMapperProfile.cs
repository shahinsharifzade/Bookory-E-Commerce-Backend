using AutoMapper;
using Bookory.Business.Utilities.DTOs.OrderItemDtos;
using Bookory.Core.Models;

namespace Bookory.Business.Utilities.Mapper
{
    public class OrderItemMapperProfile : Profile
    {
        public OrderItemMapperProfile()
        {
            CreateMap<OrderItem, OrderItemGetResponseDto>().ReverseMap();
            CreateMap<OrderItemPostDto, OrderItem>();
        }
    }
}
