using AutoMapper;
using Bookory.Business.Utilities.DTOs.BlogDtos;
using Bookory.Business.Utilities.Extension.FileExtensions.Common;
using Bookory.Core.Models;
using Microsoft.AspNetCore.Hosting;

namespace Bookory.Business.Utilities.ImageResolver.BlogResolver;

public class BlogPostImageResolver : IValueResolver<BlogPostDto, Blog, string>
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    public BlogPostImageResolver(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public string Resolve(BlogPostDto source, Blog destination, string destMember, ResolutionContext context)
    {
        string basePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "blogs");

        return FileHelper.SaveFileAsync(source.Image, basePath).GetAwaiter().GetResult();
    }
}
