using System.Net;

namespace Bookory.Business.Utilities.Exceptions.CompanyExceptions;

public sealed class CompanyCreationException : Exception, IBaseException
{
    public CompanyCreationException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.BadRequest;
    public string Message { get; set; }
}
