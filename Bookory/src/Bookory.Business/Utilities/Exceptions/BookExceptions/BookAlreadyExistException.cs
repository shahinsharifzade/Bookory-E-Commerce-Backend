using System.Net;

namespace Bookory.Business.Utilities.Exceptions.BookExceptions;

public sealed class BookAlreadyExistException : Exception, IBaseException
{
    public BookAlreadyExistException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.Conflict;
    public string Message { get; set; }
}
