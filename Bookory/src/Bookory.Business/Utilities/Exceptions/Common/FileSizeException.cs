using System.Net;

namespace Bookory.Business.Utilities.Exceptions.Common;

public sealed class FileSizeException : Exception, IBaseException
{
    public FileSizeException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.BadRequest;
    public string Message { get; set; }
}
