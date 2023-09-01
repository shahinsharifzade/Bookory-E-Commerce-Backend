using Microsoft.AspNetCore.Http;

namespace Bookory.Business.Utilities.Extension.FileExtensions.Common;
public static class FileHelper
{

    public static async Task<string> SaveFileAsync(IFormFile file, string basePath)
    {
        string filename = $"{Guid.NewGuid()}-{file.FileName}";
        string path = Path.Combine(basePath, filename);

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return filename;
    }

    public static void DeleteFile(string[] path)
    {
        var oldPath = Path.Combine(path);

        if (File.Exists(oldPath))
            File.Delete(oldPath);
    }
}
