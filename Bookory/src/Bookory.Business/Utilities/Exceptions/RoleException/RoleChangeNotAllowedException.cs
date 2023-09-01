using System.Net;

namespace Bookory.Business.Utilities.Exceptions.RoleException;

public sealed class RoleChangeNotAllowedException : Exception, IBaseException
{
    public RoleChangeNotAllowedException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.Forbidden;
    public string Message { get; set; }
}
