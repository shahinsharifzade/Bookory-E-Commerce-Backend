using AutoMapper;
using Bookory.Business.Utilities.DTOs.BasketDtos;
using Bookory.Core.Models;

namespace Bookory.Business.Utilities.Mapper
{
    public class BasketMapperProfile : Profile
    {
        public BasketMapperProfile()
        {
            CreateMap<Book, BasketItem>();
            CreateMap<BasketItem, BasketGetResponseDto>()
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForCtorParam("BasketBook", opt => opt.MapFrom(src => src.Book));
        }
    }
}
