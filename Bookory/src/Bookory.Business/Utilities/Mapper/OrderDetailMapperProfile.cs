using AutoMapper;
using Bookory.Business.Utilities.DTOs.OrderDetailDtos;
using Bookory.Core.Models;

namespace Bookory.Business.Utilities.Mapper
{
    public class OrderDetailMapperProfile : Profile
    {
        public OrderDetailMapperProfile()
        {
            CreateMap<Book, OrderItem>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailGetResponseDto>().ReverseMap();
        }
    }
}
