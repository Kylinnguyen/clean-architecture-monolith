using MediatR;

namespace CleanArchitecture.Domain.Common.Core.Events;

public interface IEvent : INotification
{
    Guid Id { get; }
    DateTimeOffset OccurredAt { get; }
}