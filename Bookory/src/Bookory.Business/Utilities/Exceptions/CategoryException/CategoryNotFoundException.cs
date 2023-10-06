using System.Net;

namespace Bookory.Business.Utilities.Exceptions.CategoryException;

public class CategoryNotFoundException : Exception, IBaseException
{
    public CategoryNotFoundException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.NotFound;
    public string Message { get; set; }
}
