using CleanArchitecture.Application.Constants;

namespace CleanArchitecture.Application.Common.Exceptions;

public class InternalServerErrorException : AppException
{
    public InternalServerErrorException(string message = "An unexpected error occurred.")
        : base(message, ErrorCodeConstants.INTERNAL_SERVER_ERROR) { }

    public InternalServerErrorException(string message, Exception innerException)
        : base(message, innerException, ErrorCodeConstants.INTERNAL_SERVER_ERROR) { }
}