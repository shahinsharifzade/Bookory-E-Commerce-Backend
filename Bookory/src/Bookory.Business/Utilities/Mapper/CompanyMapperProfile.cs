using AutoMapper;
using Bookory.Business.Utilities.DTOs.CompanyDtos;
using Bookory.Business.Utilities.ImageResolver.CompanyResolver.CompanyPostResolver;
using Bookory.Business.Utilities.ImageResolver.CompanyResolver.CompanyPutResolver;
using Bookory.Core.Models;

namespace Bookory.Business.Utilities.Mapper;

public class CompanyMapperProfile : Profile
{
	public CompanyMapperProfile()
	{
        CreateMap<CompanyPostDto, Company>()
         .ForMember(dest => dest.Logo, opt => opt.MapFrom<CompanyLogoPostResolver>())
         .ForMember(dest => dest.BannerImage, opt => opt.MapFrom<CompanyBannerImagePostResolver>());

        CreateMap<CompanyPutDto, Company>()
          .ForMember(dest => dest.Logo, opt => opt.MapFrom<CompanyLogoPutResolver>())
          .ForMember(dest => dest.BannerImage, opt => opt.MapFrom<CompanyBannerImagePutResolver>());

        CreateMap<Company, CompanyGetResponseDto>();
        CreateMap<Company, CompanyGetResponseDtoInclude>();
    }
}
