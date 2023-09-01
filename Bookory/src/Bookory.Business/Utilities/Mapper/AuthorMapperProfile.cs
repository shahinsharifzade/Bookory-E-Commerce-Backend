using AutoMapper;
using Bookory.Business.Utilities.DTOs.AuthorDtos;
using Bookory.Business.Utilities.DTOs.AuthorImageDtos;
using Bookory.Business.Utilities.Extension.FileExtensions;
using Bookory.Business.Utilities.ImageResolver.AuthorResolver;
using Bookory.Core.Models;

namespace Bookory.Business.Utilities.Mapper;

public class AuthorMapperProfile : Profile
{
    public AuthorMapperProfile()
    {
        CreateMap<AuthorImage, AuthorImageGetResponseDtoInclude>().ReverseMap(); // To include Images in Authors

        CreateMap<Author, AuthorGetResponseDtoInclude>()
            .ForCtorParam("MainImage", opt => opt.MapFrom(src => src.Images.FirstOrDefault(i => i.IsMain)!.Image))
            .ReverseMap();
        CreateMap<Author, AuthorGetResponseDto>()
           .ForCtorParam("MainImage", opt => opt.MapFrom(src => src.Images.FirstOrDefault(i => i.IsMain)!.Image))
           .ReverseMap();


        CreateMap<AuthorPostDto, Author>()
            .BeforeMap((src, dest) => src.ConditionChecker(1024))
            .ForMember(dest => dest.Images, opt => opt.MapFrom<AuthorPostImageResolver>());
        CreateMap<AuthorPutDto, Author>()
            .BeforeMap((src, dest) => src.ConditionChecker(1024))
            .ForMember(dest => dest.Images, opt => opt.MapFrom<AuthorPutImageResolver>());
    }
}
