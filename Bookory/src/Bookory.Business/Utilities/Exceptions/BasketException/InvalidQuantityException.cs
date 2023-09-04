using System.Net;

namespace Bookory.Business.Utilities.Exceptions.BasketException;

public sealed class InvalidQuantityException : Exception, IBaseException
{
    public InvalidQuantityException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.BadRequest;
    public string Message { get; set; }
}
