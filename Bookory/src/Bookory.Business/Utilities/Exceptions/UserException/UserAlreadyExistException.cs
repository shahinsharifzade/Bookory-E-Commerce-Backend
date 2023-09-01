using System.Net;

namespace Bookory.Business.Utilities.Exceptions.UserException;

public sealed class UserAlreadyExistException : Exception, IBaseException
{
    public UserAlreadyExistException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.Conflict;
    public string Message { get; set; }
}
