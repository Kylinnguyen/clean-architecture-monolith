using CleanArchitecture.Application.Constants;

namespace CleanArchitecture.Application.Common.Exceptions;

public class ValidationCustomException : AppException
{
    public IDictionary<string, string[]> Errors { get; set; }

    public ValidationCustomException(IDictionary<string, string[]> errors)
        : base("Validation failed for one or more properties.", ErrorCodeConstants.VALIDATION_ERROR, errors)
    {
        Errors = errors;
    }
}