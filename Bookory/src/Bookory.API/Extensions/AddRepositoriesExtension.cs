﻿//using Bookory.DataAccess.Repositories.Implementations;
//using Bookory.DataAccess.Repositories.Interfaces;
//using System.Reflection.Metadata;

//namespace Bookory.API.Extensions;

//public static class AddRepositoriesExtension
//{
//    public static IServiceCollection AddRepositoriesService(this IServiceCollection services)
//    {
//        services.AddScoped<IBookRepository, BookRepository>();
//        services.AddScoped<IAuthorRepository, AuthorRepository>();
//        services.AddScoped<IGenreRepository, GenreRepository>();
//        services.AddScoped<IBasketItemRepository, BasketItemRepository>();
//        services.AddScoped<IShoppingSessionRepository, ShoppingSessionRepository>();
//        services.AddScoped<IUserAdressRepository, UserAdressRepository>();

//        services.AddScoped<IPaymentDetailRepository, PaymentDetailRepository>();
//        services.AddScoped<IOrderItemRepository, OrderItemRepository>();
//        services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
//        services.AddScoped<IWishlistRepository, WishlistRepository>();
//        services.AddScoped<ICommentRepository, CommentRepository>();
//        services.AddScoped<ICompanyRepository, CompanyRepository>();

//        return services;
//    }

//}
