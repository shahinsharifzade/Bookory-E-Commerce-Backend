using AutoMapper;
using Bookory.Business.Utilities.DTOs.AuthorDtos;
using Bookory.Business.Utilities.Extension.FileExtensions.Common;
using Bookory.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Bookory.Business.Utilities.ImageResolver.AuthorResolver;

public class AuthorPostImageResolver : IValueResolver<AuthorPostDto, Author, ICollection<AuthorImage>>
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public AuthorPostImageResolver(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public ICollection<AuthorImage> Resolve(AuthorPostDto source, Author destination, ICollection<AuthorImage> destMember, ResolutionContext context)
    {
        List<AuthorImage> images = new();

        for (int i = 0; i < source.Images.Count; i++)
        {
            IFormFile imageFile = source.Images[i];

            string basePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "authors");
            string imageName = FileHelper.SaveFileAsync(imageFile, basePath).GetAwaiter().GetResult();

            AuthorImage authorImage = new AuthorImage
            {
                Image = imageName,
                IsMain = i == source.MainImageIndex,
            };

            images.Add(authorImage);
        }

        return images;
    }
}
