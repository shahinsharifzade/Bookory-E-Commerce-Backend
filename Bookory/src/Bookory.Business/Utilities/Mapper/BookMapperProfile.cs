using AutoMapper;
using Bookory.Business.Utilities.DTOs.BookDtos;
using Bookory.Business.Utilities.DTOs.BookImageDtos;
using Bookory.Business.Utilities.Extension.FileExtensions;
using Bookory.Business.Utilities.ImageResolver.BookResolver;
using Bookory.Core.Models;

namespace Bookory.Business.Utilities.Mapper;

public class BookMapperProfile : Profile
{
    public BookMapperProfile()
    {
        CreateMap<BookImage, BookImageGetResponseDtoInclude>().ReverseMap();

        CreateMap<Book, BookGetResponseDtoInclude>()
            .ForCtorParam("MainImage", opt => opt.MapFrom(src => src.Images.FirstOrDefault(i => i.IsMain)!.Image))
            .ReverseMap();

        CreateMap<Book, BookGetResponseDto>()
            .ForCtorParam("MainImage", opt => opt.MapFrom(src => src.Images.FirstOrDefault(i => i.IsMain)!.Image))
            .ForCtorParam("Genres", opt => opt.MapFrom(src => src.BookGenres.Select(bg => bg.Genre)))
            .ReverseMap();

        CreateMap<BookPutDto, Book>()
            .BeforeMap((src, dest) => src.ConditionChecker(1024))
            .ForMember(dest => dest.Images, opt => opt.MapFrom<BookPutImageResolver>());

        CreateMap<BookPostDto, Book>()
            .BeforeMap((src, dest) => src.ConditionChecker(1024))
            .ForMember(dest => dest.Images, opt => opt.MapFrom<BookPostImageResolver>())
            .ForMember(dest => dest.BookGenres, opt => opt.MapFrom((src, dest)
                => src.GenreIds.Select(genreId => new BookGenre { GenreId = genreId, BookId = dest.Id, Id = Guid.NewGuid() })));
    }
}
