using System.Net;

namespace Bookory.Business.Utilities.Exceptions.Common;

public sealed class InvalidFileTypeException : Exception, IBaseException
{

    public InvalidFileTypeException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.UnsupportedMediaType;
    public string Message { get; set; }
}
