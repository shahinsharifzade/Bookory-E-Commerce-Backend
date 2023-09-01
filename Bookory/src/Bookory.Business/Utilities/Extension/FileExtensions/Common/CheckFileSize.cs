using Microsoft.AspNetCore.Http;

namespace Bookory.Business.Utilities.Extension.FileExtensions.Common;

public static class CheckFileSize
{
    public static bool CheckSize(this IFormFile file, int size)
    {
        return file.Length / 1024 < size;
    }
}
