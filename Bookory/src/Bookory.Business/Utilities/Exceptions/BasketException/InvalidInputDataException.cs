using System.Net;

namespace Bookory.Business.Utilities.Exceptions.BasketException;

public sealed class InvalidInputDataException : Exception, IBaseException
{
    public InvalidInputDataException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.BadRequest;
    public string Message { get; set; }
}