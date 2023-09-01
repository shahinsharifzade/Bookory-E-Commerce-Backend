using System.Net;

namespace Bookory.Business.Utilities.Exceptions.GenreExceptions;

public class GenreNotFoundException : Exception, IBaseException
{
    public GenreNotFoundException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.NotFound;
    public string Message { get; set; }
}
