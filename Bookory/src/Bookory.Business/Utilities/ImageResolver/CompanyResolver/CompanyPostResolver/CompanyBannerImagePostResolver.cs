using AutoMapper;
using Bookory.Business.Utilities.DTOs.CompanyDtos;
using Bookory.Business.Utilities.Extension.FileExtensions.Common;
using Bookory.Core.Models;
using Microsoft.AspNetCore.Hosting;

namespace Bookory.Business.Utilities.ImageResolver.CompanyResolver.CompanyPostResolver;

public class CompanyBannerImagePostResolver : IValueResolver<CompanyPostDto, Company, string>
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    public CompanyBannerImagePostResolver(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }


    public string Resolve(CompanyPostDto source, Company destination, string destMember, ResolutionContext context)
    {
        string basePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "companies");
        return FileHelper.SaveFileAsync(source.BannerImage, basePath).GetAwaiter().GetResult();
    }
}
