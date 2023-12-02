using AutoMapper;
using Bookory.Business.Utilities.DTOs.BlogDtos;
using Bookory.Business.Utilities.ImageResolver.BlogResolver;
using Bookory.Core.Models;

namespace Bookory.Business.Utilities.Mapper;

public class BlogMapperProfile : Profile
{
	public BlogMapperProfile()
	{
        CreateMap<BlogGetResponseDto, Blog>().ReverseMap();

        CreateMap<BlogPutDto, Blog>()
            .ForMember(src => src.Image, opt => opt.MapFrom<BlogPutImageResolver>());
        CreateMap<BlogPostDto, Blog>()
            .ForMember(src => src.Image, opt => opt.MapFrom<BlogPostImageResolver>());
    }
}
