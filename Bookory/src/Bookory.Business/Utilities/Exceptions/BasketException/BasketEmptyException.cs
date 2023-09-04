namespace Bookory.Business.Utilities.Exceptions.BasketException;

public sealed class BasketEmptyException : Exception, IBaseException
{
    public BasketEmptyException(string message) : base(message)
    {
        Message = message;
    }

    public int StatusCode { get ; set ; }
    public string Message { get ; set ; }
}
