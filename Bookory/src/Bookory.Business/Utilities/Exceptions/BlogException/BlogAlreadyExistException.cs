using System.Net;

namespace Bookory.Business.Utilities.Exceptions.BlogException;

public sealed class BlogAlreadyExistException : Exception, IBaseException
{
    public BlogAlreadyExistException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.Conflict;
    public string Message { get; set; }
}
