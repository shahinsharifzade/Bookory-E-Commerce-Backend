using System.Net;
namespace Bookory.Business.Utilities.Exceptions.OrderDetailExceptions;

public sealed class OrderDetailNotFoundException : Exception, IBaseException
{
    public OrderDetailNotFoundException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.NotFound;
    public string Message { get; set; }
}
