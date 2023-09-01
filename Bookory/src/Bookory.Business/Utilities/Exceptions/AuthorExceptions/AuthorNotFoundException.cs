using System.Net;

namespace Bookory.Business.Utilities.Exceptions.AuthorExceptions;

public class AuthorNotFoundException : Exception, IBaseException
{
    public AuthorNotFoundException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.NotFound;
    public string Message { get; set; }
}
