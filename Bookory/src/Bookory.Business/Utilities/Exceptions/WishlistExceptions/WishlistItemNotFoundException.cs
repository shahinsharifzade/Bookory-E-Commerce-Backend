using System.Net;

namespace Bookory.Business.Utilities.Exceptions.WishlistExceptions;

public sealed class WishlistItemNotFoundException : Exception, IBaseException
{
    public WishlistItemNotFoundException(string message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.NotFound;
    public string Message { get; set; }
}
