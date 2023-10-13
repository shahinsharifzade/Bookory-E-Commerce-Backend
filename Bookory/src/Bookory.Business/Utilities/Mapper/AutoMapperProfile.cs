using AutoMapper;
using Bookory.Business.Utilities.DTOs.AuthorDtos;
using Bookory.Business.Utilities.DTOs.AuthorImageDtos;
using Bookory.Business.Utilities.DTOs.BasketDtos;
using Bookory.Business.Utilities.DTOs.BlogDtos;
using Bookory.Business.Utilities.DTOs.BookDtos;
using Bookory.Business.Utilities.DTOs.BookImageDtos;
using Bookory.Business.Utilities.DTOs.CategoryDtos;
using Bookory.Business.Utilities.DTOs.CommentDtos;
using Bookory.Business.Utilities.DTOs.CompanyDtos;
using Bookory.Business.Utilities.DTOs.ContactDtos;
using Bookory.Business.Utilities.DTOs.GenreDtos;
using Bookory.Business.Utilities.DTOs.OrderDetailDtos;
using Bookory.Business.Utilities.DTOs.OrderItemDtos;
using Bookory.Business.Utilities.DTOs.PaymentDetailDto;
using Bookory.Business.Utilities.DTOs.UserAddressDtos;
using Bookory.Business.Utilities.DTOs.UserDtos;
using Bookory.Business.Utilities.DTOs.WishlistDtos;
using Bookory.Business.Utilities.Extension.FileExtensions;
using Bookory.Business.Utilities.ImageResolver.AuthorResolver;
using Bookory.Business.Utilities.ImageResolver.BlogResolver;
using Bookory.Business.Utilities.ImageResolver.BookResolver;
using Bookory.Business.Utilities.ImageResolver.CompanyResolver.CompanyPostResolver;
using Bookory.Business.Utilities.ImageResolver.CompanyResolver.CompanyPutResolver;
using Bookory.Core.Models;
using Bookory.Core.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Bookory.Business.Utilities.Mapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {

        #region Book 

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

        #region Author 

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

        #region Genre 

        CreateMap<Genre, GenreGetResponseDtoInclude>();
        CreateMap<Genre, GenreGetResponeDto>()
            .ForCtorParam("Books", opt => opt.MapFrom(src => src.BookGenres.Select(bg => bg.Book)))
            .ReverseMap();
        CreateMap<GenrePostDto, Genre>();
        CreateMap<GenrePutDto, Genre>();

        #endregion

        #region User 

        CreateMap<RegisterDto, AppUser>();
        CreateMap<AppUser, UserGetResponseDto>().ReverseMap();
        CreateMap<IdentityRole, RoleGetResponseDto>().ReverseMap();

        #endregion

        #region Basket 

        CreateMap<Book, BasketItem>();
        CreateMap<BasketItem, BasketGetResponseDto>()
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForCtorParam("BasketBook", opt => opt.MapFrom(src => src.Book));

        #endregion

        #region UserAddress 

        CreateMap<UserAddress, UserAddressGetReponseDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId)).ReverseMap();

        CreateMap<UserAddressPostDto, UserAddress>().ReverseMap();

        CreateMap<UserAddressPutDto, UserAddress>();

        #endregion

        #region Payment Detail 

        CreateMap<PaymentDetail, PaymentDetailGetResponseDto>().ReverseMap();
        CreateMap<PaymentDetailPostDto, PaymentDetail>();

        #endregion

        #region Order Detail 

        CreateMap<OrderDetailPostDto, OrderDetail>();

        CreateMap<Book, OrderItem>().ReverseMap();
        CreateMap<OrderDetail, OrderDetailGetResponseDto>().ReverseMap();

        #endregion

        #region Order Item 

        CreateMap<OrderItem, OrderItemGetResponseDto>().ReverseMap();
        CreateMap<OrderItemPostDto, OrderItem>();

        #endregion

        #region Wishlist 

        CreateMap<Wishlist, WishlistGetResponseDto>();

        #endregion

        #region Comment 

        CreateMap<CommentPostDto, Comment>();
        CreateMap<Comment, CommentGetResponseDto>().ReverseMap();

        #endregion

        #region Company 

        CreateMap<CompanyPostDto, Company>()
            .ForMember(dest => dest.Logo, opt => opt.MapFrom<CompanyLogoPostResolver>())
            .ForMember(dest => dest.BannerImage, opt => opt.MapFrom<CompanyBannerImagePostResolver>());

        CreateMap<CompanyPutDto, Company>()
          .ForMember(dest => dest.Logo, opt => opt.MapFrom<CompanyLogoPutResolver>())
          .ForMember(dest => dest.BannerImage, opt => opt.MapFrom<CompanyBannerImagePutResolver>());

        CreateMap<Company, CompanyGetResponseDto>();
        CreateMap<Company, CompanyGetResponseDtoInclude>();

        #endregion

        #region Category

        CreateMap<CategoryGetReponseDto, Category>().ReverseMap();
        CreateMap<CategoryPostDto, Category>();
        CreateMap<CategoryPutDto, Category>();

        #endregion

        #region Blog

        CreateMap<BlogGetResponseDto, Blog>().ReverseMap();

        CreateMap<BlogPutDto, Blog>()
            .ForMember(src => src.Image , opt => opt.MapFrom<BlogPutImageResolver>());
        CreateMap<BlogPostDto, Blog>()
            .ForMember(src => src.Image , opt => opt.MapFrom<BlogPostImageResolver>());

        #endregion

        #region Contact

        CreateMap<ContactGetResponseDto, Contact>().ReverseMap();
        CreateMap<ContactPostDto, Contact>();

        #endregion
    }
}


