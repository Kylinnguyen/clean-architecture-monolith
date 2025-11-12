namespace CleanArchitecture.Domain.Common.Core.Events;

public interface IDomainEvent : IEvent
{
    /// <summary>
    /// Correlates the domain event to the command (or transaction) that triggered it.
    /// </summary>
    Guid? CommandId { get; }
}