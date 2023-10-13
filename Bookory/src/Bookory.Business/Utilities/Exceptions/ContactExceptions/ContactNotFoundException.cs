using System.Net;

namespace Bookory.Business.Utilities.Exceptions.ContactExceptions;


public sealed class ContactNotFoundException : Exception, IBaseException
{
    public ContactNotFoundException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.NotFound;
    public string Message { get; set; }
}
