using CleanArchitecture.Domain.Common.Core.Events;

namespace CleanArchitecture.Domain.Common.Core.BaseEntities;

public abstract class AggregateRoot : Entity, IAggregateRoot
{
    public ICollection<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
    public void AddDomainEvent(DomainEvent eventItem)
    {
        DomainEvents.Add(eventItem);
    }

    public void RemoveDomainEvent(DomainEvent eventItem)
    {
        DomainEvents.Remove(eventItem);        
    }

    public void ClearDomainEvents()
    {
        DomainEvents.Clear();        
    }
}

public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>
{
    public ICollection<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
    public void AddDomainEvent(DomainEvent eventItem)
    {
        DomainEvents.Add(eventItem);
    }

    public void RemoveDomainEvent(DomainEvent eventItem)
    {
        DomainEvents.Remove(eventItem);        
    }

    public void ClearDomainEvents()
    {
        DomainEvents.Clear();        
    }
}