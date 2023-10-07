using System.Net;

namespace Bookory.Business.Utilities.Exceptions.BlogException;

public sealed class BlogCreationException : Exception, IBaseException
{
    public BlogCreationException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.Forbidden;
    public string Message { get; set; }
}
