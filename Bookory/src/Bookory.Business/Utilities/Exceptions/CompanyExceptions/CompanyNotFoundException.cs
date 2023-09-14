using System.Net;
namespace Bookory.Business.Utilities.Exceptions.CompanyExceptions;

public sealed class CompanyNotFoundException : Exception, IBaseException
{
    public CompanyNotFoundException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.NotFound;
    public string Message { get; set; }
}
