using System.Net;

namespace Bookory.Business.Utilities.Exceptions.AuthorExceptions;

public sealed class AuthorAlreadyExistException : Exception, IBaseException
{
    public AuthorAlreadyExistException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.Conflict;
    public string Message { get; set; }
}
