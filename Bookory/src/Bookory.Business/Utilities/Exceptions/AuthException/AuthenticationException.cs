using System.Net;

namespace Bookory.Business.Utilities.Exceptions.AuthException;

public sealed class AuthenticationFailedException : Exception , IBaseException
{
    public AuthenticationFailedException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.Unauthorized;
    public string Message { get; set; }
}
