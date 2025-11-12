namespace CleanArchitecture.Domain.Common.Core.BaseEntities;

public interface IEntity
{
    object?[] GetKeys();
}

public interface IEntity<TKey> : IEntity
{
    TKey Id { get; set; }
}