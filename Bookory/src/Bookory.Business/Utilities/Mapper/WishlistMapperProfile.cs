using AutoMapper;
using Bookory.Business.Utilities.DTOs.WishlistDtos;
using Bookory.Core.Models;

namespace Bookory.Business.Utilities.Mapper;

public class WishlistMapperProfile : Profile
{
	public WishlistMapperProfile()
	{
        CreateMap<Wishlist, WishlistGetResponseDto>();

    }
}
