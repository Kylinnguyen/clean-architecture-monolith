using CleanArchitecture.Application.Constants;

namespace CleanArchitecture.Application.Common.Exceptions;

public class BadRequestException : AppException
{
    public object? Errors { get; set; }

    public BadRequestException(string message = "Bad Request", object? errors = null)
        : base(message, ErrorCodeConstants.BAD_REQUEST)
    {
        Errors = errors;
    }
}