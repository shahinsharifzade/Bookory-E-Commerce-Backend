using System.Net;

namespace Bookory.Business.Utilities.Exceptions.UserAddressException;

public sealed class UserAddressNotFoundException : Exception, IBaseException
{
    public UserAddressNotFoundException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.NotFound;
    public string Message { get; set; }
}

