using CleanArchitecture.Domain.Common.Core.Events;

namespace CleanArchitecture.Domain.Common.Core.BaseEntities;

public interface IHasDomainEvents
{
    ICollection<DomainEvent> DomainEvents { get; }
    void AddDomainEvent(DomainEvent eventItem);
    void RemoveDomainEvent(DomainEvent eventItem);
    void ClearDomainEvents();
}