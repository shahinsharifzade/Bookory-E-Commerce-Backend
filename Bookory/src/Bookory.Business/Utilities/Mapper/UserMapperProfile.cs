using AutoMapper;
using Bookory.Business.Utilities.DTOs.UserDtos;
using Bookory.Core.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Bookory.Business.Utilities.Mapper
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {

            CreateMap<RegisterDto, AppUser>();
            CreateMap<AppUser, UserGetResponseDto>().ReverseMap();
            CreateMap<IdentityRole, RoleGetResponseDto>().ReverseMap();
        }
    }
}
