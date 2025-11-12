namespace CleanArchitecture.Application.Common.Exceptions;

public class AppException : Exception
{
    public string? ErrorCode { get; }
    public object? Details { get; }

    protected AppException(string message, string? errorCode = null, object? details = null)
        : base(message)
    {
        ErrorCode = errorCode;
        Details = details;
    }

    protected AppException(string message, Exception innerException,
            string? errorCode = null, object? details = null)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        Details = details;
    }
}