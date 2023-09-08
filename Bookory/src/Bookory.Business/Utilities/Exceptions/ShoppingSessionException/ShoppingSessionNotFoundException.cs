using System.Net;

namespace Bookory.Business.Utilities.Exceptions.ShoppingSessionException;

public sealed class ShoppingSessionNotFoundException : Exception, IBaseException
{
    public ShoppingSessionNotFoundException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.NotFound;
    public string Message { get; set; }
}