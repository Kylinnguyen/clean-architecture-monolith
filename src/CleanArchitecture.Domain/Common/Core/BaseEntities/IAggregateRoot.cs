namespace CleanArchitecture.Domain.Common.Core.BaseEntities;

public interface IAggregateRoot : IEntity, IHasDomainEvents
{

}

public interface IAggregateRoot<TKey> : IEntity<TKey>, IAggregateRoot
{
    
}