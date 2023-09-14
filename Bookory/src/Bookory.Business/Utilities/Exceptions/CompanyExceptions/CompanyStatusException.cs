using System.Net;
namespace Bookory.Business.Utilities.Exceptions.CompanyExceptions;


public sealed class CompanyStatusException : Exception, IBaseException
{
    public CompanyStatusException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.BadRequest;
    public string Message { get; set; }
}