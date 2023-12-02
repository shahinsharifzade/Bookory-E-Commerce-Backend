using AutoMapper;
using Bookory.Business.Utilities.DTOs.UserAddressDtos;
using Bookory.Core.Models;

namespace Bookory.Business.Utilities.Mapper
{
    public class UserAddressMapperProfile : Profile
    {
        public UserAddressMapperProfile()
        {
            CreateMap<UserAddress, UserAddressGetReponseDto>()
           .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId)).ReverseMap();

            CreateMap<UserAddressPostDto, UserAddress>().ReverseMap();

            CreateMap<UserAddressPutDto, UserAddress>();
        }
    }
}
