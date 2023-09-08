using System.Net;
namespace Bookory.Business.Utilities.Exceptions.BasketItemException;

public sealed class BasketItemQuantityNotEnoughException : Exception, IBaseException
{
    public BasketItemQuantityNotEnoughException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.NotFound;
    public string Message { get; set; }
}

