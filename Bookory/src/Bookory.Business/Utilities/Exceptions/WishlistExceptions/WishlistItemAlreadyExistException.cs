using System.Net;

namespace Bookory.Business.Utilities.Exceptions.WishlistExceptions;

public sealed class WishlistItemAlreadyExistException : Exception, IBaseException
{
    public WishlistItemAlreadyExistException(string message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.Conflict;
    public string Message { get; set; }
}
