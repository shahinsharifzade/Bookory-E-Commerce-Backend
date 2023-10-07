using AutoMapper;
using Bookory.Business.Utilities.DTOs.BookDtos;
using Bookory.Business.Utilities.Extension.FileExtensions.Common;
using Bookory.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Bookory.Business.Utilities.ImageResolver.BookResolver;

public class BookPostImageResolver : IValueResolver<BookPostDto, Book, ICollection<BookImage>>
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    public BookPostImageResolver(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public ICollection<BookImage> Resolve(BookPostDto source, Book destination, ICollection<BookImage> destMember, ResolutionContext context)
    {
        List<BookImage> images = new();

        for (int i = 0; i < source.Images.Count; i++)
        {
            IFormFile file = source.Images[i];

            string basePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "books");
            string fileName = FileHelper.SaveFileAsync(file, basePath).GetAwaiter().GetResult();

            BookImage newImage = new BookImage
            {
                Image = fileName,
                IsMain = i == source.MainImageIndex
            };

            images.Add(newImage);

        }

        return images;
    }
}
