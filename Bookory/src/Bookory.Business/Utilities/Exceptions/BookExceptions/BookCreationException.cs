using System.Net;   
namespace Bookory.Business.Utilities.Exceptions.BookExceptions;

public sealed class BookCreationException : Exception, IBaseException
{
    public BookCreationException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.Forbidden;
    public string Message { get; set; }
}
