namespace CleanArchitecture.Domain.Common.Core.BaseEntities;

public interface IEntity
{
}

public interface IEntity<TKey> : IEntity
{
    TKey Id { get; set; }
}