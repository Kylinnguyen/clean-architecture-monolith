using CleanArchitecture.Domain.Common.Core.Auditing;

namespace CleanArchitecture.Domain.Common.Core.BaseEntities;

public interface IAuditableEntity: IAuditable, IEntity
{

}

public interface IAuditableEntity<TKey> : IAuditableEntity, IEntity<TKey>
{
    
}