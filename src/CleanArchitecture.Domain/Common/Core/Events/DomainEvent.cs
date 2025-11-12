namespace CleanArchitecture.Domain.Common.Core.Events;

public abstract class DomainEvent : IDomainEvent
{
    protected DomainEvent(Guid? commandId = null)
    {
        Id = Guid.NewGuid();
        OccurredAt = DateTimeOffset.UtcNow;
        CommandId = commandId;
    }

    public Guid Id { get; }
    public DateTimeOffset OccurredAt { get; }
    public Guid? CommandId { get; set; }
}