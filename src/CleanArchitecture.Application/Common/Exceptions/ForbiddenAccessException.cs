using CleanArchitecture.Application.Constants;

namespace CleanArchitecture.Application.Common.Exceptions;

public class ForbiddenAccessException : AppException
{
    public string? Message { get; set; }

    public ForbiddenAccessException(string message = "Access denied")
        : base(message, ErrorCodeConstants.FORBIDDEN)
    {
        Message = message;
    }
}

