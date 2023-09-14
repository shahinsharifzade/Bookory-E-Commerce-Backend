using System.Net;

namespace Bookory.Business.Utilities.Exceptions.AuthException;

public sealed class BlockedUserException : Exception, IBaseException
{
    public BlockedUserException(string message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.NotFound;
    public string Message { get; set; }
}