using System.Net;
namespace Bookory.Business.Utilities.Exceptions.AuthException;

public sealed class ConfirmEmailException : Exception, IBaseException
{
    public ConfirmEmailException(string message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.Forbidden;
    public string Message { get; set; }
}
