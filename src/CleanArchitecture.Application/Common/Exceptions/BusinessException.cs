
namespace CleanArchitecture.Application.Common.Exceptions;

public class BusinessException : AppException
{
    public string Message { get; set; }
    public string? ErrorCode { get; set; }
    public object? Details { get; set; }

    public BusinessException(string message)
        : base(message)
    {
        Message = message;
    }
    
    public BusinessException(string message, string errorCode)
        : base(message, errorCode)
    {
        Message = message;
        ErrorCode = errorCode;
    }

    public BusinessException(string message, string? errorCode = null, object? details = null)
    : base(message, errorCode, details)
    {
        Message = message;
        ErrorCode = errorCode;
        Details = details;
    }
    
    public BusinessException(string message, string errorCode, Exception? innerException = null)
        : base(message, errorCode, innerException)
    {
        Message = message;
        ErrorCode = errorCode;
    }
}