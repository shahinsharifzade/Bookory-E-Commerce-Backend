using AutoMapper;
using Bookory.Business.Utilities.DTOs.GenreDtos;
using Bookory.Core.Models;

namespace Bookory.Business.Utilities.Mapper;

public class GenreMapperProfile : Profile
{
    public GenreMapperProfile()
    {
        CreateMap<Genre, GenreGetResponseDtoInclude>();
        CreateMap<Genre, GenreGetResponeDto>()
            .ForCtorParam("Books", opt => opt.MapFrom(src => src.BookGenres.Select(bg => bg.Book)))
            .ReverseMap();
        CreateMap<GenrePostDto, Genre>();
        CreateMap<GenrePutDto, Genre>();
    }
}
