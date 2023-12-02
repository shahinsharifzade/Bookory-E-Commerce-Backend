using AutoMapper;
using Bookory.Business.Utilities.DTOs.ContactDtos;
using Bookory.Core.Models;

namespace Bookory.Business.Utilities.Mapper;

public class ContactMapperProfile : Profile
{
	public ContactMapperProfile()
	{
        CreateMap<ContactGetResponseDto, Contact>().ReverseMap();
        CreateMap<ContactPostDto, Contact>();

    }
}
