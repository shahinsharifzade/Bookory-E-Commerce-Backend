using System.Net;

namespace Bookory.Business.Utilities.Exceptions.BookExceptions;

public sealed class BookNotFoundException : Exception, IBaseException
{
    public BookNotFoundException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.NotFound;
    public string Message { get; set; }
}
