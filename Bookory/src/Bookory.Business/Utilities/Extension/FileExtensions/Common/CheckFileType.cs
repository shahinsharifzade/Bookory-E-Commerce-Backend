using Microsoft.AspNetCore.Http;
namespace Bookory.Business.Utilities.Extension.FileExtensions.Common;

public static class CheckFileType
{
    public static bool CheckType(this IFormFile file, string type)
    {
        return file.ContentType.Contains($"{type}/");
    }
}
