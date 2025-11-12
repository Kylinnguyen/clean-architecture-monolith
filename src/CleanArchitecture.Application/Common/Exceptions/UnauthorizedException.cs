using CleanArchitecture.Application.Constants;

namespace CleanArchitecture.Application.Common.Exceptions;

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message = "Unauthorized")
        : base(message, ErrorCodeConstants.UNAUTHORIZED) { }
}