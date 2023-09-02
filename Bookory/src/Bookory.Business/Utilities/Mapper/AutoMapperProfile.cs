using AutoMapper;
using Bookory.Business.Utilities.DTOs.AuthorDtos;
using Bookory.Business.Utilities.DTOs.AuthorImageDtos;
using Bookory.Business.Utilities.DTOs.BasketDtos;
using Bookory.Business.Utilities.DTOs.BookDtos;
using Bookory.Business.Utilities.DTOs.BookImageDtos;
using Bookory.Business.Utilities.DTOs.GenreDtos;
using Bookory.Business.Utilities.DTOs.UserDtos;
using Bookory.Business.Utilities.Extension.FileExtensions;
using Bookory.Business.Utilities.ImageResolver.AuthorResolver;
using Bookory.Business.Utilities.ImageResolver.BookResolver;
using Bookory.Core.Models;
using Bookory.Core.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Bookory.Business.Utilities.Mapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {

        #region Book Mapper

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

        #endregion

        #region Author Mapper

        CreateMap<AuthorImage, AuthorImageGetResponseDtoInclude>().ReverseMap();

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

        #endregion

        #region Genre Mapper

        CreateMap<Genre, GenreGetResponseDtoInclude>();
        CreateMap<Genre, GenreGetResponeDto>()
            .ForCtorParam("Books", opt => opt.MapFrom(src => src.BookGenres.Select(bg => bg.Book)))
            .ReverseMap();
        CreateMap<GenrePostDto, Genre>();
        CreateMap<GenrePutDto, Genre>();

        #endregion


        #region User Mapper

        CreateMap<RegisterDto, AppUser>();
        CreateMap<AppUser, UserGetResponseDto>().ReverseMap();
        CreateMap<IdentityRole, RoleGetResponseDto>().ReverseMap();

        #endregion

        #region Basket Mapper

        CreateMap<Book, BasketItem>();
        CreateMap<BasketItem, BasketGetResponseDto>()
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForCtorParam("BasketBook", opt => opt.MapFrom(src => src.Book));

        #endregion
    }
}


