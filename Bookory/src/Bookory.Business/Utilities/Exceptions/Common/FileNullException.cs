namespace Bookory.Business.Utilities.Exceptions.Common;

public sealed class FileNullException : Exception, IBaseException
{
    private static readonly string StaticMessage = "File is null.";

    public FileNullException() : base(StaticMessage)
    {
        Message = StaticMessage;
    }

    public int StatusCode { get; set; }
    public string Message { get; set; }

}
