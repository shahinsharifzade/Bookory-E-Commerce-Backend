using System.Net;

namespace Bookory.Business.Utilities.Exceptions.CategoryException;

public class CategoryAlreadyExistExeption : Exception, IBaseException
{
    public CategoryAlreadyExistExeption(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get; set; } = (int)HttpStatusCode.Conflict;
    public string Message { get; set; }
}
