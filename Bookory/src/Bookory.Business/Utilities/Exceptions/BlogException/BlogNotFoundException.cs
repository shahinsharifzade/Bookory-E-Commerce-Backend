using System.Net;

namespace Bookory.Business.Utilities.Exceptions.BlogException;

public sealed class BlogNotFoundException : Exception, IBaseException
{
    public BlogNotFoundException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.NotFound;
    public string Message { get; set; }
}
