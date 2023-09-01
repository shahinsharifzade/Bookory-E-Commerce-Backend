namespace Bookory.Business.Utilities.Exceptions.Common;

public sealed class NullOrWhitespaceArgumentException : Exception, IBaseException
{
    public NullOrWhitespaceArgumentException(string message)
    {
        Message = message;
    }
    public int StatusCode { get ; set ; }
    public string Message { get ; set ; }
}
