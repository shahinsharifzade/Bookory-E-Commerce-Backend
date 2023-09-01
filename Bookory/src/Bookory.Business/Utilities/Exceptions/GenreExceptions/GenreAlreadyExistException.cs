using System.Net;

namespace Bookory.Business.Utilities.Exceptions.GenreExceptions;

public class GenreAlreadyExistException : Exception, IBaseException
{
    public GenreAlreadyExistException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.Conflict;
    public string Message { get; set; }
}
