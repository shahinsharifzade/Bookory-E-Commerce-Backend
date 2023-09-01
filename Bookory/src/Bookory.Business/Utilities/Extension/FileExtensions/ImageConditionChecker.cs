using Bookory.Business.Utilities.Exceptions.Common;
using Bookory.Business.Utilities.Extension.FileExtensions.Common;
using Microsoft.AspNetCore.Http;

namespace Bookory.Business.Utilities.Extension.FileExtensions;

public static class ImageConditionChecker
{
    public static void ConditionChecker<TDto>(this TDto dto, int fileMaxSize)
    {
        foreach (var image in dto.GetType().GetProperty("Images").GetValue(dto) as IEnumerable<IFormFile>)
        {
            if (image == null)
                throw new FileNullException();

            if (!image.CheckSize(fileMaxSize))
                throw new FileSizeException($"Image size should not exceed {fileMaxSize} KB.");

            if (!image.CheckType("image"))
                throw new InvalidFileTypeException("The provided file is not of the expected image type.");
        }
    }
}
