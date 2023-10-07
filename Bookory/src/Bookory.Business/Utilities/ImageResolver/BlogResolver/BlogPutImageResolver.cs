using AutoMapper;
using Bookory.Business.Utilities.DTOs.BlogDtos;
using Bookory.Business.Utilities.Extension.FileExtensions.Common;
using Bookory.Core.Models;
using Microsoft.AspNetCore.Hosting;

namespace Bookory.Business.Utilities.ImageResolver.BlogResolver;

public class BlogPutImageResolver : IValueResolver<BlogPutDto, Blog, string>
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    public BlogPutImageResolver(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public string Resolve(BlogPutDto source, Blog destination, string destMember, ResolutionContext context)
    {
          if (!string.IsNullOrEmpty(destination.Image))
        {
            string previousImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "blogs", destination.Image);

            FileHelper.DeleteFile(new string[] { previousImagePath });

            destination.Image = null;
        }

        string basePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "blogs");

        return FileHelper.SaveFileAsync(source.Image, basePath).GetAwaiter().GetResult();
    }
}
