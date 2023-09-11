using System.Net;

namespace Bookory.Business.Utilities.Exceptions.CommentExceptions;
public sealed class CommentNotFoundException : Exception, IBaseException
{
    public CommentNotFoundException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.NotFound;
    public string Message { get; set; }
}
