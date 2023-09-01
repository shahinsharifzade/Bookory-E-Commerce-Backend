using System.Net;

namespace Bookory.Business.Utilities.Exceptions.LoginException;

public class LoginFailedException : Exception, IBaseException
{
    public LoginFailedException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.BadRequest;
    public string Message { get; set; }
}
