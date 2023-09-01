using System.Net;

namespace Bookory.Business.Utilities.Exceptions.AuthException;

public sealed class UserUpdateFailedException : Exception, IBaseException
{
    public UserUpdateFailedException(string message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.BadRequest;
    public string Message { get; set; }
}
