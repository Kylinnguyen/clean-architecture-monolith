namespace CleanArchitecture.Domain.Common.Core.BaseEntities;

public abstract class Entity : IEntity
{
}


public abstract class Entity<TKey> : Entity, IEntity<TKey>
{
    public virtual TKey Id { get; set; }
}
