using AutoMapper;
using Bookory.Business.Utilities.DTOs.CategoryDtos;
using Bookory.Core.Models;

namespace Bookory.Business.Utilities.Mapper;

public class CategoryMapperProfile : Profile
{
	public CategoryMapperProfile()
	{
        CreateMap<CategoryGetReponseDto, Category>().ReverseMap();
        CreateMap<CategoryPostDto, Category>();
        CreateMap<CategoryPutDto, Category>();
    }
}
