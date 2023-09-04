using System.Net;

namespace Bookory.Business.Utilities.Exceptions.BasketException;

public sealed class BasketItemNotFoundException : Exception, IBaseException
{
    public BasketItemNotFoundException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.NotFound;
    public string Message { get; set; }
}

