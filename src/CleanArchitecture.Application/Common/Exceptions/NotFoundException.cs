using CleanArchitecture.Application.Constants;

namespace CleanArchitecture.Application.Common.Exceptions;

public class NotFoundException : AppException
{
    public object? EntityId { get; set; }
    public string EntityName { get; set; }

    public string? Message { get; set; }

    public NotFoundException(string entityName, object entityId)
        : base($"{entityName} with key '{entityId}' was not found.", ErrorCodeConstants.NOT_FOUND)
    {
        EntityId = entityId;
        entityName = entityName;
    }

    public NotFoundException(string entityName, string? message)
        : base(message ?? $"{entityName} was not found.", ErrorCodeConstants.NOT_FOUND)
    {
        Message = message;
    }
}