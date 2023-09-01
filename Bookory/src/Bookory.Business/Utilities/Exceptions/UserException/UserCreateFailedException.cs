using System.Net;

namespace Bookory.Business.Utilities.Exceptions.UserException;

public sealed class UserCreateFailedException : Exception, IBaseException
{
    public UserCreateFailedException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.BadRequest;
    public string Message { get; set; }

}
