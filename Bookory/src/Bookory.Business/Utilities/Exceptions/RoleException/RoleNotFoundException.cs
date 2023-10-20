using Bookory.Business.Utilities.Exceptions;
using System.Net;

namespace Bookory.Business.Utilities.Exceptions.RoleException;

public class RoleNotFoundException : Exception, IBaseException
{
    public RoleNotFoundException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.NotFound;
    public string Message { get; set; }
}
